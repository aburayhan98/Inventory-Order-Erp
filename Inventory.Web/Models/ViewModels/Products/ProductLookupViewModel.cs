namespace Inventory.Web.Models.ViewModels.Products;

public class ProductLookupViewModel
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Sku { get; set; } = string.Empty;

	public decimal Price { get; set; }

	public int QuantityInStock { get; set; }
}