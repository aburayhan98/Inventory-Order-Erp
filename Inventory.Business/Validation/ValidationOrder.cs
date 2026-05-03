

using Inventory.Business.DTOs.Orders;

namespace Inventory.Business.Validation;

public static class ValidationOrder
{
	public static void ValidateOrder(CreateOrderDto dto)
	{
		if (dto == null)
			throw new ArgumentNullException(nameof(dto));

		if (string.IsNullOrWhiteSpace(dto.CustomerName))
			throw new ArgumentException("Customer name is required.");

		if (dto.CustomerName.Length > 150)
			throw new ArgumentException("Customer name cannot exceed 150 characters.");

		if (dto.Items is null || dto.Items.Count == 0)
			throw new ArgumentException("Order must contain at least one item.");

		if (dto.Items.Any(x => x.ProductId <= 0))
			throw new ArgumentException("Invalid product selected.");

		if (dto.Items.Any(x => x.Quantity <= 0))
			throw new ArgumentException("Quantity must be greater than zero.");
	}
}
