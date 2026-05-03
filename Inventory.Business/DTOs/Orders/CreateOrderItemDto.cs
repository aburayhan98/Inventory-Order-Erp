namespace Inventory.Business.DTOs.Orders;
public class CreateOrderItemDto
{
	public int ProductId { get; set; }
	public int Quantity { get; set; }
}