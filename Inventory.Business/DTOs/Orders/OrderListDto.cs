namespace Inventory.Business.DTOs.Orders;

public class OrderListDto
{
	public int Id { get; set; }

	public string CustomerName { get; set; } = string.Empty;

	public decimal TotalAmount { get; set; }

	public DateTime OrderDate { get; set; }
}