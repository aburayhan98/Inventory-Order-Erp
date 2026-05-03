namespace Inventory.Web.Models.ViewModels.Orders;

public class OrderDetailsViewModel
{
	public int Id { get; set; }

	public string CustomerName { get; set; } = string.Empty;

	public DateTime OrderDate { get; set; }

	public decimal TotalAmount { get; set; }

	public IReadOnlyList<OrderDetailsItemViewModel> Items { get; set; }
			= new List<OrderDetailsItemViewModel>();
}