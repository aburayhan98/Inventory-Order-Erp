using Inventory.Web.Models.ViewModels.Products;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models.ViewModels.Orders;

public  class OrderCreateViewModel
{
	[Required(ErrorMessage = "Customer name is required.")]
	[MaxLength(150, ErrorMessage = "Customer name cannot exceed 150 characters.")]
	public string CustomerName { get; set; } = string.Empty;

	public List<OrderItemViewModel> Items { get; set; } = new()
		{
				new OrderItemViewModel()
		};

	public IReadOnlyList<ProductLookupViewModel> Products { get; set; }
			= [];
}