
namespace Inventory.Domain.Models;

public class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}