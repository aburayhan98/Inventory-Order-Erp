using Inventory.Business.DTOs.Orders;
using Inventory.Business.Interfaces.Persistence.IOrder;
using Inventory.Business.Interfaces.Persistence.IServices;
using Inventory.Business.Validation;

namespace Inventory.Business.Services;

public class OrderService(
		IOrderCommand orderCommand,
		IOrderQuery orderQuery) : IOrderService
{
	private readonly IOrderCommand _orderCommand = orderCommand;
	private readonly IOrderQuery _orderQuery = orderQuery;

	public Task<IEnumerable<OrderListDto>> GetListAsync(
			CancellationToken cancellationToken = default)
	{
		return _orderQuery.GetListAsync(cancellationToken);
	}

	public async Task<OrderDetailsDto?> GetDetailsAsync(
			int id,
			CancellationToken cancellationToken = default)
	{
		if (id <= 0)
			throw new ArgumentException("Invalid order id.");

		return await _orderQuery.GetDetailsAsync(id, cancellationToken);
	}

	public async Task<int> CreateAsync(
			CreateOrderDto dto,
			CancellationToken cancellationToken = default)
	{
		ValidationOrder.ValidateOrder(dto);

		var normalizedDto = new CreateOrderDto
		{
			CustomerName = dto.CustomerName.Trim(),
			Items = dto.Items
						.Where(x => x.Quantity > 0)
						.GroupBy(x => x.ProductId)
						.Select(g => new CreateOrderItemDto
						{
							ProductId = g.Key,
							Quantity = g.Sum(x => x.Quantity)
						})
						.ToList()
		};

		return await _orderCommand.CreateAsync(normalizedDto, cancellationToken);
	}
}