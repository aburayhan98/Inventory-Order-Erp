using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers;

public class ProductController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
