using Dapper;
using Inventory.Business.DTOs.Products;
using Inventory.Business.Interfaces.Persistence.IProduct;
using Inventory.DataAccess.Connection;
using Microsoft.Data.SqlClient;

namespace Inventory.DataAccess.Data.Commands;

public class ProductCommand(DbConfig dbConfig) : IProductCommand
{
	public async Task<int> CreateAsync(
			ProductCreateDto dto,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                INSERT INTO Products
                    (Name, SKU, Price, QuantityInStock)
                OUTPUT INSERTED.Id
                VALUES
                    (@Name, @Sku, @Price, @QuantityInStock);";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.ExecuteScalarAsync<int>(
					query,
					new
					{
						Name = dto.Name.Trim(),
						Sku = dto.Sku.Trim(),
						dto.Price,
						dto.QuantityInStock
					});
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<int> UpdateAsync(
			ProductUpdateDto dto,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                UPDATE Products
                SET
                    Name = @Name,
                    SKU = @Sku,
                    Price = @Price,
                    QuantityInStock = @QuantityInStock
                WHERE Id = @Id;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.ExecuteAsync(
					query,
					new
					{
						dto.Id,
						Name = dto.Name.Trim(),
						Sku = dto.Sku.Trim(),
						dto.Price,
						dto.QuantityInStock
					});
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<int> DeleteAsync(
			int id,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                DELETE FROM Products
                WHERE Id = @Id;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.ExecuteAsync(query, new { Id = id });
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}
}