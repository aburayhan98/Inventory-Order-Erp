namespace Inventory.Business.Validation;

public static class ValidationProduct
{
	public static void Validate(
		string name,
		string sku,
		decimal price,
		int quantity)
{
	if (string.IsNullOrWhiteSpace(name))
		throw new ArgumentException("Product name is required.");

	if (name.Length > 150)
		throw new ArgumentException("Product name cannot exceed 150 characters.");

	if (string.IsNullOrWhiteSpace(sku))
		throw new ArgumentException("SKU is required.");

	if (sku.Length > 50)
		throw new ArgumentException("SKU cannot exceed 50 characters.");

	if (price < 0)
		throw new ArgumentException("Price cannot be negative.");

	if (quantity < 0)
		throw new ArgumentException("Quantity cannot be negative.");
}
}
