using Inventory.Business.DTOs.Orders;
using Inventory.DataAccess.Data.Commands;
using Inventory.DataAccess.Data.Queries;
using Inventory.Test;
using Inventory.Test.Attributes;


namespace Inventory.Test.QueryTests;

public class OrderQueryTests
{
	private readonly OrderCommand _orderCommand;
	private readonly OrderQuery _uot;
	private readonly ProductCommand _productCommand;

	public OrderQueryTests()
	{
		var config = new ConfigTest();

		_orderCommand = new OrderCommand(config.DbConfig);
		_uot = new OrderQuery(config.DbConfig);
		_productCommand = new ProductCommand(config.DbConfig);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task GetList()
	{
		// Arrange
		var product = CommonObjectInit.Product;
		var productId = await _productCommand.CreateAsync(product, CancellationToken.None);

		var orderDto = new CreateOrderDto
		{
			CustomerName = "Test Customer",
			Items = new List<CreateOrderItemDto>
						{
								new() { ProductId = productId, Quantity = 1 }
						}
		};

		var orderId = await _orderCommand.CreateAsync(orderDto, CancellationToken.None);

		// Act
		var orders = await _uot.GetListAsync(CancellationToken.None);

		// Assert
		Assert.NotNull(orders);
		Assert.Contains(orders, x => x.Id == orderId);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task GetDetails()
	{
		// Arrange
		var product = CommonObjectInit.Product;
		var productId = await _productCommand.CreateAsync(product, CancellationToken.None);

		var orderDto = new CreateOrderDto
		{
			CustomerName = "Details Customer",
			Items = new List<CreateOrderItemDto>
						{
								new() { ProductId = productId, Quantity = 2 }
						}
		};

		var orderId = await _orderCommand.CreateAsync(orderDto, CancellationToken.None);

		// Act
		var order = await _uot.GetDetailsAsync(orderId, CancellationToken.None);

		// Assert
		Assert.NotNull(order);
		Assert.Equal(orderId, order.Id);
		Assert.Equal(orderDto.CustomerName, order.CustomerName);
		Assert.Single(order.Items);

		var item = order.Items.First();

		Assert.Equal(productId, item.ProductId);
		Assert.Equal(product.Name, item.ProductName);
		Assert.Equal(product.Sku, item.Sku);
		Assert.Equal(2, item.Quantity);
		Assert.Equal(product.Price, item.UnitPrice);
	}

	[FactInDebugOnly]
	[WithRollback]
	public async Task GetDetails_ShouldReturnNull_WhenOrderDoesNotExist()
	{
		// Arrange
		var invalidOrderId = -999;

		// Act
		var order = await _uot.GetDetailsAsync(invalidOrderId, CancellationToken.None);

		// Assert
		Assert.Null(order);
	}
}