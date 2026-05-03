namespace Inventory.Business.DTOs.Orders;

public class OrderDetailsDto
{
	public int Id { get; set; }

	public string CustomerName { get; set; } = string.Empty;

	public DateTime OrderDate { get; set; }

	public decimal TotalAmount { get; set; }

	public List<OrderItemDetailsDto> Items { get; set; } = new();
}