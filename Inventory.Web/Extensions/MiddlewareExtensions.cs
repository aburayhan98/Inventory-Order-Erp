using Inventory.Web.Middleware;

namespace Inventory.Web.Extensions;

public static class MiddlewareExtensions
{
	public static IApplicationBuilder UseGlobalExceptionHandling(
			this IApplicationBuilder app)
	{
		return app.UseMiddleware<ExceptionHandlingMiddleware>();
	}
}