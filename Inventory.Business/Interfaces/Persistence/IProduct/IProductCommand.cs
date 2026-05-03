using Inventory.Business.DTOs.Products;

namespace Inventory.Business.Interfaces.Persistence.IProduct;

public interface IProductCommand
{
	/// <summary>
	/// Creates a new product and returns the generated Id.
	/// </summary>
	Task<int> CreateAsync(
			ProductCreateDto dto,
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing product.
	/// Returns number of affected rows.
	/// </summary>
	Task<int> UpdateAsync(
			ProductUpdateDto dto,
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a product by Id.
	/// Returns number of affected rows.
	/// </summary>
	Task<int> DeleteAsync(
			int id,
			CancellationToken cancellationToken = default);
}