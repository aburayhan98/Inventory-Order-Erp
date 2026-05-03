using Inventory.Business.DTOs.Products;
using Inventory.Web.Models.ViewModels.Orders;

public class OrderCreateViewModel
{
	public string CustomerName { get; set; } = string.Empty;

	public List<OrderItemViewModel> Items { get; set; } = new();

	public List<ProductLookupDto> Products { get; set; } = new(); // ✅ FIX
}