using Inventory.Business.DTOs.Products;
using Inventory.Business.Interfaces.Persistence.IProduct;
using Inventory.Business.Interfaces.Persistence.IServices;
using Inventory.Business.Validation;
using Inventory.Domain.Exception;
using Inventory.Domain.Exceptions;

namespace Inventory.Business.Services;

public sealed class ProductService(
		IProductCommand productCommand,
		IProductQuery productQuery) : IProductService
{
	private readonly IProductCommand _productCommand = productCommand;
	private readonly IProductQuery _productQuery = productQuery;

	public Task<IEnumerable<ProductListDto>> GetListAsync(
			CancellationToken cancellationToken = default)
	{
		return _productQuery.GetListAsync(cancellationToken);
	}

	public Task<IEnumerable<ProductListDto>> SearchAsync(
			string term,
			CancellationToken cancellationToken = default)
	{
		return _productQuery.SearchAsync(term, cancellationToken);
	}

	public Task<ProductDetailsDto?> GetAsync(
			int id,
			CancellationToken cancellationToken = default)
	{
		return _productQuery.GetAsync(id, cancellationToken);
	}

	public Task<IEnumerable<ProductLookupDto>> GetLookupAsync(
			CancellationToken cancellationToken = default)
	{
		return _productQuery.GetLookupAsync(cancellationToken);
	}
	public async Task<int> CreateAsync(
			ProductCreateDto dto,
			CancellationToken cancellationToken = default)
	{
		ValidationProduct.Validate(dto.Name, dto.Sku, dto.Price, dto.QuantityInStock);

		var exists = await _productQuery.IsSkuExistsAsync(
				dto.Sku,
				null,
				cancellationToken);

		if (exists)
			throw new DuplicateSkuException(dto.Sku);

		return await _productCommand.CreateAsync(dto, cancellationToken);
	}

	public async Task<int> UpdateAsync(
			ProductUpdateDto dto,
			CancellationToken cancellationToken = default)
	{
		if (dto.Id <= 0)
			throw new ArgumentException("Invalid product id.");

		ValidationProduct.Validate(dto.Name, dto.Sku, dto.Price, dto.QuantityInStock);

		var existing = await _productQuery.GetAsync(dto.Id, cancellationToken);

		if (existing is null)
			throw new NotFoundException($"Product with id {dto.Id} not found.");

		var exists = await _productQuery.IsSkuExistsAsync(
				dto.Sku,
				dto.Id,
				cancellationToken);

		if (exists)
			throw new DuplicateSkuException(dto.Sku);

		return await _productCommand.UpdateAsync(dto, cancellationToken);
	}

	public async Task<int> DeleteAsync(
			int id,
			CancellationToken cancellationToken = default)
	{
		if (id <= 0)
			throw new ArgumentException("Invalid product id.");

		var existing = await _productQuery.GetAsync(id, cancellationToken);

		if (existing is null)
			throw new NotFoundException($"Product with id {id} not found.");

		return await _productCommand.DeleteAsync(id, cancellationToken);
	}
}