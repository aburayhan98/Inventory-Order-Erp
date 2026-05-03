using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers
{
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
