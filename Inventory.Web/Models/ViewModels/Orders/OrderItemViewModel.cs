using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models.ViewModels.Orders;

public class OrderItemViewModel
{
	[Required(ErrorMessage = "Product is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Please select a valid product.")]
	public int ProductId { get; set; }

	[Required(ErrorMessage = "Quantity is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
	public int Quantity { get; set; }
}