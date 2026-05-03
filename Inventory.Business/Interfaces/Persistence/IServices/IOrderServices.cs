using Inventory.Business.DTOs.Orders;

namespace Inventory.Business.Interfaces.Persistence.IServices;

public interface IOrderService
{
	Task<IEnumerable<OrderListDto>> GetListAsync(
			CancellationToken cancellationToken = default);

	Task<OrderDetailsDto?> GetDetailsAsync(
			int id,
			CancellationToken cancellationToken = default);

	Task<int> CreateAsync(
			CreateOrderDto dto,
			CancellationToken cancellationToken = default);
}