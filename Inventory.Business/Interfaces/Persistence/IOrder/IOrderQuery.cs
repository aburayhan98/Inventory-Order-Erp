using Inventory.Business.DTOs.Orders;

namespace Inventory.Business.Interfaces.Persistence.IOrder;

public interface IOrderQuery
{
	/// <summary>
	/// Gets all orders for the order list page.
	/// </summary>
	Task<IEnumerable<OrderListDto>> GetListAsync(
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets full order details including items.
	/// </summary>
	Task<OrderDetailsDto?> GetDetailsAsync(
			int id,
			CancellationToken cancellationToken = default);
}