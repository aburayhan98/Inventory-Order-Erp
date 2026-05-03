namespace Inventory.Web.Models.ViewModels.Orders;

public class OrderDetailsItemViewModel
{
	public int ProductId { get; set; }

	public string ProductName { get; set; } = string.Empty;

	public string Sku { get; set; } = string.Empty;

	public int Quantity { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal LineTotal => Quantity * UnitPrice;
}