using Inventory.Business.DTOs.Orders;

namespace Inventory.Business.Interfaces.Persistence.IOrder;

public interface IOrderCommand
{
	/// <summary>
	/// Creates an order with multiple order items,
	/// calculates total amount, and reduces product stock.
	/// Returns the generated Order Id.
	/// </summary>
	Task<int> CreateAsync(
			CreateOrderDto dto,
			CancellationToken cancellationToken = default);
}