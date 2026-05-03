
namespace Inventory.DataAccess.Data.Common;
public class ProductStockRow
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int QuantityInStock { get; set; }
}