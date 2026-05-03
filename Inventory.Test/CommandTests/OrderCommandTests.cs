using Inventory.Business.DTOs.Orders;
using Inventory.DataAccess.Data.Commands;
using Inventory.DataAccess.Data.Queries;
using Inventory.Test.Attributes;


namespace Inventory.Test.CommandTests;

public class OrderCommandTests
{
	private readonly OrderCommand _uot;
	private readonly OrderQuery _query;
	private readonly ProductCommand _productCommand;
	private readonly ProductQuery _productQuery;

	public OrderCommandTests()
	{
		var config = new ConfigTest();

		_uot = new OrderCommand(config.DbConfig);
		_query = new OrderQuery(config.DbConfig);

		_productCommand = new ProductCommand(config.DbConfig);
		_productQuery = new ProductQuery(config.DbConfig);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task Create()
	{
		// =========================
		// Arrange
		// =========================

		// Create products first (required for order)
		var product1 = CommonObjectInit.Product;
		var product2 = CommonObjectInit.Product;

		var productId1 = await _productCommand.CreateAsync(product1, CancellationToken.None);
		var productId2 = await _productCommand.CreateAsync(product2, CancellationToken.None);

		var orderDto = new CreateOrderDto
		{
			CustomerName = "Test Customer",
			Items = new List<CreateOrderItemDto>
						{
								new() { ProductId = productId1, Quantity = 2 },
								new() { ProductId = productId2, Quantity = 1 }
						}
		};

		// =========================
		// Act
		// =========================

		var orderId = await _uot.CreateAsync(orderDto, CancellationToken.None);

		// =========================
		// Assert
		// =========================

		Assert.True(orderId > 0);

		var order = await _query.GetDetailsAsync(orderId, CancellationToken.None);

		Assert.NotNull(order);
		Assert.Equal(orderDto.CustomerName, order.CustomerName);
		Assert.Equal(2, order.Items.Count);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task Create_ShouldReduceStock()
	{
		// =========================
		// Arrange
		// =========================

		var product = CommonObjectInit.Product;

		var productId = await _productCommand.CreateAsync(product, CancellationToken.None);

		var before = await _productQuery.GetAsync(productId, CancellationToken.None);

		var orderDto = new CreateOrderDto
		{
			CustomerName = "Stock Test",
			Items = new List<CreateOrderItemDto>
						{
								new() { ProductId = productId, Quantity = 2 }
						}
		};

		// =========================
		// Act
		// =========================

		await _uot.CreateAsync(orderDto, CancellationToken.None);

		var after = await _productQuery.GetAsync(productId, CancellationToken.None);

		// =========================
		// Assert
		// =========================

		Assert.NotNull(before);
		Assert.NotNull(after);

		Assert.Equal(before.QuantityInStock - 2, after.QuantityInStock);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task Create_ShouldThrow_WhenInsufficientStock()
	{
		// =========================
		// Arrange
		// =========================

		var product = CommonObjectInit.Product;

		var productId = await _productCommand.CreateAsync(product, CancellationToken.None);

		var existing = await _productQuery.GetAsync(productId, CancellationToken.None);

		var orderDto = new CreateOrderDto
		{
			CustomerName = "Fail Test",
			Items = new List<CreateOrderItemDto>
						{
								new() { ProductId = productId, Quantity = existing.QuantityInStock + 10 }
						}
		};

		// =========================
		// Act & Assert
		// =========================

		await Assert.ThrowsAsync<Exception>(() =>
				_uot.CreateAsync(orderDto, CancellationToken.None));
	}
}