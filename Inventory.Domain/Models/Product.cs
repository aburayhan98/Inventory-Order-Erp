
namespace Inventory.Domain.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string SKU { get; set; }

    public decimal Price { get; set; }

    public int QuantityInStock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}