using Dapper;
using Inventory.Business.DTOs.Orders;
using Inventory.Business.Interfaces.Persistence.IOrder;
using Inventory.DataAccess.Connection;
using Inventory.DataAccess.Data.Common;
using Microsoft.Data.SqlClient;

namespace Inventory.DataAccess.Data.Commands;

public class OrderCommand(DbConfig dbConfig) : IOrderCommand
{
	public async Task<int> CreateAsync(
			CreateOrderDto dto,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;
		SqlTransaction transaction = null;

		try
		{
			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			transaction = connection.BeginTransaction();

			var normalizedItems = dto.Items
					.Where(x => x.Quantity > 0)
					.GroupBy(x => x.ProductId)
					.Select(g => new CreateOrderItemDto
					{
						ProductId = g.Key,
						Quantity = g.Sum(x => x.Quantity)
					})
					.ToList();

			if (normalizedItems.Count == 0)
				throw new InvalidOperationException("Order must contain at least one item.");

			var productIds = normalizedItems
					.Select(x => x.ProductId)
					.Distinct()
					.ToArray();

			var productQuery = @"
                SELECT
                    Id,
                    Name,
                    Price,
                    QuantityInStock
                FROM Products WITH (UPDLOCK, HOLDLOCK)
                WHERE Id IN @ProductIds;";

			var products = (await connection.QueryAsync<ProductStockRow>(
					productQuery,
					new { ProductIds = productIds },
					transaction)).ToList();

			var productMap = products.ToDictionary(x => x.Id);

			decimal totalAmount = 0;

			foreach (var item in normalizedItems)
			{
				if (!productMap.TryGetValue(item.ProductId, out var product))
					throw new InvalidOperationException($"Product id {item.ProductId} was not found.");

				if (product.QuantityInStock < item.Quantity)
					throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'.");

				totalAmount += product.Price * item.Quantity;
			}

			var orderQuery = @"
                INSERT INTO Orders
                    (CustomerName, OrderDate, TotalAmount)
                OUTPUT INSERTED.Id
                VALUES
                    (@CustomerName, GETDATE(), @TotalAmount);";

			var orderId = await connection.ExecuteScalarAsync<int>(
					orderQuery,
					new
					{
						CustomerName = dto.CustomerName.Trim(),
						TotalAmount = totalAmount
					},
					transaction);

			foreach (var item in normalizedItems)
			{
				var product = productMap[item.ProductId];

				var orderItemQuery = @"
                    INSERT INTO OrderItems
                        (OrderId, ProductId, Quantity, UnitPrice)
                    VALUES
                        (@OrderId, @ProductId, @Quantity, @UnitPrice);";

				await connection.ExecuteAsync(
						orderItemQuery,
						new
						{
							OrderId = orderId,
							ProductId = item.ProductId,
							Quantity = item.Quantity,
							UnitPrice = product.Price
						},
						transaction);

				var stockUpdateQuery = @"
                    UPDATE Products
                    SET QuantityInStock = QuantityInStock - @Quantity
                    WHERE Id = @ProductId
                      AND QuantityInStock >= @Quantity;";

				var affectedRows = await connection.ExecuteAsync(
						stockUpdateQuery,
						new
						{
							ProductId = item.ProductId,
							Quantity = item.Quantity
						},
						transaction);

				if (affectedRows != 1)
					throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'.");
			}

			transaction.Commit();

			return orderId;
		}
		catch
		{
			transaction?.Rollback();
			throw;
		}
		finally
		{
			transaction?.Dispose();
			connection?.Dispose();
		}
	}
}