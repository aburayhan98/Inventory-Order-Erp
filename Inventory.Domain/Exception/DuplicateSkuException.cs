namespace Inventory.Domain.Exception;

public sealed class DuplicateSkuException : System.Exception
{
	public string Sku { get; }

	public DuplicateSkuException(string sku)
			: base($"A product with SKU '{sku}' already exists.")
	{
		Sku = sku;
	}

	public DuplicateSkuException(string sku, System.Exception innerException)
			: base($"A product with SKU '{sku}' already exists.", innerException)
	{
		Sku = sku;
	}
}