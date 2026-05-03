using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers;

public sealed class HomeController : Controller
{
	public IActionResult Error(string? message)
	{
		ViewBag.ErrorMessage = message ?? "Something went wrong.";
		return View();
	}
}