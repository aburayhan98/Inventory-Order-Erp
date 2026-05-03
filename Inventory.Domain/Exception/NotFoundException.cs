using System;

namespace Inventory.Domain.Exceptions;

public sealed class NotFoundException : System.Exception
{
	public NotFoundException(string message)
			: base(message)
	{
	}

	public NotFoundException(string message, System.Exception innerException)
			: base(message, innerException)
	{
	}
}