using Inventory.Domain.Exception;
using Inventory.Domain.Exceptions;
using System.Net;

namespace Inventory.Web.Middleware;

public sealed class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(
			RequestDelegate next,
			ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (DuplicateSkuException ex)
		{
			_logger.LogWarning(ex, ex.Message);
			await HandleRedirectAsync(context, ex.Message);
		}
		catch (NotFoundException ex)
		{
			_logger.LogWarning(ex, ex.Message);
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			await HandleRedirectAsync(context, ex.Message);
		}
		catch (ArgumentException ex)
		{
			_logger.LogWarning(ex, ex.Message);
			await HandleRedirectAsync(context, ex.Message);
		}
		catch (InvalidOperationException ex)
		{
			_logger.LogWarning(ex, ex.Message);
			await HandleRedirectAsync(context, ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception occurred.");
			await HandleRedirectAsync(context, "Something went wrong. Please try again.");
		}
	}

	private static Task HandleRedirectAsync(HttpContext context, string message)
	{
		context.Response.Redirect($"/Home/Error?message={Uri.EscapeDataString(message)}");
		return Task.CompletedTask;
	}
}