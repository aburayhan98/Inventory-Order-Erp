using Inventory.Business.DTOs.Orders;

namespace Inventory.Business.Interfaces.Persistence.IServices;

public interface IOrderService
{
	Task<IEnumerable<OrderListDto>> GetListAsync(
			CancellationToken cancellationToken = default);

	Task<OrderDetailsDto?> GetDetailsAsync(
			int id,
			CancellationToken cancellationToken = default);

	/// <summary>
	/// Create Order and OrderItems in a single transaction, returns the created Order's Id
	/// </summary>
	/// <param name="dto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CreateAsync(
			CreateOrderDto dto,
			CancellationToken cancellationToken = default);
	
}