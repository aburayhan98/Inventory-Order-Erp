using Inventory.Business.DTOs.Orders;
using Inventory.Business.DTOs.Products;

namespace Inventory.Test;

public static class CommonObjectInit
{
	// =========================
	// PRODUCT
	// =========================

	public static ProductCreateDto Product => new()
	{
		Name = "Test Product",
		Sku = $"SKU-{Guid.NewGuid():N}", // ✅ dynamic
		Price = 100,
		QuantityInStock = 10
	};

	public static ProductUpdateDto ProductUpdate(int id, string sku) => new()
	{
		Id = id,
		Name = "Updated Product",
		Sku = sku,
		Price = 200,
		QuantityInStock = 20
	};

	// =========================
	// ORDER
	// =========================

	public static CreateOrderDto Order => new()
	{
		CustomerName = "Test Customer",
		Items = new List<CreateOrderItemDto>
				{
						new() { ProductId = 1, Quantity = 2 },
						new() { ProductId = 2, Quantity = 1 }
				}
	};
}