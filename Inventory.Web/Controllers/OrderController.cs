using Inventory.Business.DTOs.Orders;
using Inventory.Business.Interfaces.Persistence.IServices;
using Inventory.Web.Models.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers;

[Authorize]
public sealed class OrderController : Controller
{
	private readonly IOrderService _orderService;
	private readonly IProductService _productService;

	public OrderController(
			IOrderService orderService,
			IProductService productService)
	{
		_orderService = orderService;
		_productService = productService;
	}

	[HttpGet]
	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		var orders = await _orderService.GetListAsync(cancellationToken);
		return View(orders);
	}

	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken cancellationToken)
	{
		var products = await _productService.GetLookupAsync(cancellationToken);

		var model = new OrderCreateViewModel
		{
			Items = new List<OrderItemViewModel>
						{
								new OrderItemViewModel()
						},
			Products = products.ToList()
		};

		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
			OrderCreateViewModel model,
			CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
		{
			model.Products = (await _productService.GetLookupAsync(cancellationToken)).ToList();
			return View(model);
		}

		try
		{
			var dto = new CreateOrderDto
			{
				CustomerName = model.CustomerName,
				Items = model.Items
							.Where(x => x.ProductId > 0 && x.Quantity > 0)
							.Select(x => new CreateOrderItemDto
							{
								ProductId = x.ProductId,
								Quantity = x.Quantity
							})
							.ToList()
			};

			var orderId = await _orderService.CreateAsync(dto, cancellationToken);

			TempData["Success"] = "Order created successfully.";
			return RedirectToAction(nameof(Details), new { id = orderId });
		}
		catch (System.Exception ex)
		{
			ModelState.AddModelError(string.Empty, ex.Message);
			model.Products = (await _productService.GetLookupAsync(cancellationToken)).ToList();
			return View(model);
		}
	}

	[HttpGet]
	public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
	{
		var orderDto = await _orderService.GetDetailsAsync(id, cancellationToken);

		if (orderDto == null)
		{
			return NotFound();
		}

		var model = new OrderDetailsViewModel
		{
			Id = orderDto.Id,
			CustomerName = orderDto.CustomerName,
			OrderDate = orderDto.OrderDate,
			TotalAmount = orderDto.TotalAmount,
			Items = orderDto.Items.Select(i => new OrderDetailsItemViewModel
			{
				ProductId = i.ProductId,
				ProductName = i.ProductName,
				Sku = i.Sku,
				Quantity = i.Quantity,
				UnitPrice = i.UnitPrice
			}).ToList()
		};

		return View(model);
	}
}