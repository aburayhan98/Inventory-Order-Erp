using Xunit;

namespace Inventory.Test.Attributes;

public sealed class FactInDebugOnlyAttribute : FactAttribute
{
	public FactInDebugOnlyAttribute()
	{
#if !DEBUG
        Skip = "This test runs only in DEBUG mode.";
#endif
	}
}