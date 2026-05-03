using Dapper;
using Inventory.Business.DTOs.Products;
using Inventory.Business.Interfaces.Persistence.IProduct;
using Inventory.DataAccess.Connection;
using Microsoft.Data.SqlClient;

namespace Inventory.DataAccess.Data.Queries;

public class ProductQuery(DbConfig dbConfig) : IProductQuery
{
	public async Task<IEnumerable<ProductListDto>> GetListAsync(
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                SELECT
                    Id,
                    Name,
                    SKU AS Sku,
                    Price,
                    QuantityInStock,
                    CreatedAt
                FROM Products
                ORDER BY CreatedAt DESC;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.QueryAsync<ProductListDto>(query);
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<IEnumerable<ProductListDto>> SearchAsync(
			string term,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                SELECT
                    Id,
                    Name,
                    SKU AS Sku,
                    Price,
                    QuantityInStock,
                    CreatedAt
                FROM Products
                WHERE @Term IS NULL
                   OR Name LIKE '%' + @Term + '%'
                   OR SKU LIKE '%' + @Term + '%'
                ORDER BY CreatedAt DESC;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.QueryAsync<ProductListDto>(
					query,
					new
					{
						Term = string.IsNullOrWhiteSpace(term) ? null : term.Trim()
					});
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<ProductDetailsDto> GetAsync(
			int id,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                SELECT
                    Id,
                    Name,
                    SKU AS Sku,
                    Price,
                    QuantityInStock,
                    CreatedAt
                FROM Products
                WHERE Id = @Id;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.QuerySingleOrDefaultAsync<ProductDetailsDto>(
					query,
					new { Id = id });
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<IEnumerable<ProductLookupDto>> GetLookupAsync(
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                SELECT
                    Id,
                    Name,
                    SKU AS Sku,
                    Price,
                    QuantityInStock
                FROM Products
                WHERE QuantityInStock > 0
                ORDER BY Name;";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			return await connection.QueryAsync<ProductLookupDto>(query);
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}

	public async Task<bool> IsSkuExistsAsync(
			string sku,
			int? excludeProductId,
			CancellationToken cancellationToken)
	{
		SqlConnection connection = null;

		try
		{
			var query = @"
                SELECT COUNT(1)
                FROM Products
                WHERE SKU = @Sku
                  AND (@ExcludeProductId IS NULL OR Id <> @ExcludeProductId);";

			connection = dbConfig.Connection;
			await connection.OpenAsync(cancellationToken);

			var count = await connection.ExecuteScalarAsync<int>(
					query,
					new
					{
						Sku = sku.Trim(),
						ExcludeProductId = excludeProductId
					});

			return count > 0;
		}
		catch { throw; }
		finally { connection?.Dispose(); }
	}
}