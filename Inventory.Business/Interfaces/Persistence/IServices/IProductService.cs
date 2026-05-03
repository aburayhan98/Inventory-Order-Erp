using Inventory.Business.DTOs.Products;

namespace Inventory.Business.Interfaces.Persistence.IServices;

public interface IProductService
{
	Task<IEnumerable<ProductListDto>> GetListAsync(
			CancellationToken cancellationToken = default);

	Task<IEnumerable<ProductListDto>> SearchAsync(
			string term,
			CancellationToken cancellationToken = default);

	Task<ProductDetailsDto?> GetAsync(
			int id,
			CancellationToken cancellationToken = default);

	Task<IEnumerable<ProductLookupDto>> GetLookupAsync(
			CancellationToken cancellationToken = default);

	Task<int> CreateAsync(
			ProductCreateDto dto,
			CancellationToken cancellationToken = default);

	Task<int> UpdateAsync(
			ProductUpdateDto dto,
			CancellationToken cancellationToken = default);

	Task<int> DeleteAsync(
			int id,
			CancellationToken cancellationToken = default);
}