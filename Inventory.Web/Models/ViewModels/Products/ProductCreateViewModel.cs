using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models.ViewModels.Products;

public class ProductCreateViewModel
{
	[Required(ErrorMessage = "Product name is required.")]
	[MaxLength(150, ErrorMessage = "Product name cannot exceed 150 characters.")]
	public string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "SKU is required.")]
	[MaxLength(50, ErrorMessage = "SKU cannot exceed 50 characters.")]
	public string Sku { get; set; } = string.Empty;

	[Required]
	[Range(0.01, 99999999, ErrorMessage = "Price must be greater than 0.")]
	public decimal Price { get; set; }

	[Required]
	[Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
	public int QuantityInStock { get; set; }
}