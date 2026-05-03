using Inventory.Business.DTOs.Products;
using Inventory.DataAccess.Data.Commands;
using Inventory.DataAccess.Data.Queries;
using Inventory.Test.Attributes;

namespace Inventory.Test.CommandTests;

public class ProductCommandTests
{
	private readonly ProductCommand _uot;
	private readonly ProductQuery _query;
	private readonly ProductCreateDto _createDto;

	public ProductCommandTests()
	{
		var config = new ConfigTest();

		_uot = new ProductCommand(config.DbConfig);
		_query = new ProductQuery(config.DbConfig);

		_createDto = CommonObjectInit.Product;
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task Create()
	{
		// Arrange
		var isSkuExists = await _query.IsSkuExistsAsync(
				_createDto.Sku,
				null,
				CancellationToken.None);

		Assert.False(isSkuExists);

		// Act
		var productId = await _uot.CreateAsync(
				_createDto,
				CancellationToken.None);

		// Assert
		Assert.True(productId > 0);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task Update()
	{
		// Arrange
		var productId = await _uot.CreateAsync(
				_createDto,
				CancellationToken.None);

		var updateDto = new ProductUpdateDto
		{
			Id = productId,
			Name = "Updated Product",
			Sku = _createDto.Sku,
			Price = 250,
			QuantityInStock = 30
		};

		// Act
		var affectedRows = await _uot.UpdateAsync(
				updateDto,
				CancellationToken.None);

		// Assert
		Assert.Equal(1, affectedRows);

		var product = await _query.GetAsync(
				productId,
				CancellationToken.None);

		Assert.NotNull(product);
		Assert.Equal(updateDto.Name, product.Name);
		Assert.Equal(updateDto.Price, product.Price);
		Assert.Equal(updateDto.QuantityInStock, product.QuantityInStock);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task Delete()
	{
		// Arrange
		var productId = await _uot.CreateAsync(
				_createDto,
				CancellationToken.None);

		// Act
		var affectedRows = await _uot.DeleteAsync(
				productId,
				CancellationToken.None);

		// Assert
		Assert.Equal(1, affectedRows);

		var product = await _query.GetAsync(
				productId,
				CancellationToken.None);

		Assert.Null(product);
	}
}