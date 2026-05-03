using Inventory.Business.DTOs.Products;

namespace Inventory.Business.Interfaces.Persistence.IProduct;

public interface IProductQuery
{
	/// <summary>
	/// Gets all products for product list page.
	/// </summary>
	Task<IEnumerable<ProductListDto>> GetListAsync(
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets products by name or SKU for AJAX search.
	/// </summary>
	Task<IEnumerable<ProductListDto>> SearchAsync(
			string term,
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a single product by Id.
	/// </summary>
	Task<ProductDetailsDto?> GetAsync(
			int id,
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets products for order dropdown.
	/// </summary>
	Task<IEnumerable<ProductLookupDto>> GetLookupAsync(
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Checks whether SKU already exists.
	/// Use excludeProductId during update.
	/// </summary>
	Task<bool> IsSkuExistsAsync(
			string sku,
			int? excludeProductId = null,
			CancellationToken cancellationToken = default);
}