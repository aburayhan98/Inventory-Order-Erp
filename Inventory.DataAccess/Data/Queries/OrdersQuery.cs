using Dapper;
using Inventory.Business.DTOs.Orders;
using Inventory.Business.Interfaces.Persistence.IOrder;
using Inventory.DataAccess.Connection;
using Microsoft.Data.SqlClient;

namespace Inventory.DataAccess.Data.Queries;

public class OrderQuery(DbConfig dbConfig) : IOrderQuery
{
	public async Task<IEnumerable<OrderListDto>> GetListAsync(
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                SELECT
                    Id,
                    CustomerName,
                    TotalAmount,
                    OrderDate
                FROM Orders
                ORDER BY OrderDate DESC;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.QueryAsync<OrderListDto>(query);
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<OrderDetailsDto> GetDetailsAsync(
			int id,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var orderQuery = @"
                SELECT
                    Id,
                    CustomerName,
                    OrderDate,
                    TotalAmount
                FROM Orders
                WHERE Id = @Id;";

			var itemsQuery = @"
                SELECT
                    oi.ProductId,
                    p.Name AS ProductName,
                    p.SKU AS Sku,
                    oi.Quantity,
                    oi.UnitPrice
                FROM OrderItems oi
                INNER JOIN Products p ON p.Id = oi.ProductId
                WHERE oi.OrderId = @OrderId
                ORDER BY oi.Id;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			var order = await connection.QuerySingleOrDefaultAsync<OrderDetailsDto>(
					orderQuery,
					new { Id = id });

			if (order is null)
				return null;

			var items = await connection.QueryAsync<OrderItemDetailsDto>(
					itemsQuery,
					new { OrderId = id });

			order.Items = items.ToList();

			return order;
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}
}