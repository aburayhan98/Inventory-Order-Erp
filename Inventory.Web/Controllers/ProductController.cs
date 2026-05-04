using Inventory.Business.DTOs.Products;
using Inventory.Business.Interfaces.Persistence.IServices;
using Inventory.Web.Models.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Controllers;

[Authorize]
public sealed class ProductController(IProductService productService) : Controller
{
	private readonly IProductService _productService = productService;

	[HttpGet]
	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		var products = await _productService.GetListAsync(cancellationToken);
		return View(products);
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View(new ProductCreateViewModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
			ProductCreateViewModel model,
			CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		try
		{
			await _productService.CreateAsync(new ProductCreateDto
			{
				Name = model.Name,
				Sku = model.Sku,
				Price = model.Price,
				QuantityInStock = model.QuantityInStock
			}, cancellationToken);

			TempData["Success"] = "Product created successfully.";
			return RedirectToAction(nameof(Index));
		}
		catch (System.Exception ex)
		{
			ModelState.AddModelError(string.Empty, ex.Message);
			return View(model);
		}
	}

	[HttpGet]
	public async Task<IActionResult> Edit(
			int id,
			CancellationToken cancellationToken)
	{
		var product = await _productService.GetAsync(id, cancellationToken);

		if (product is null)
			return NotFound();

		return View(new ProductEditViewModel
		{
			Id = product.Id,
			Name = product.Name,
			Sku = product.Sku,
			Price = product.Price,
			QuantityInStock = product.QuantityInStock
		});
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
			ProductEditViewModel model,
			CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		try
		{
			await _productService.UpdateAsync(new ProductUpdateDto
			{
				Id = model.Id,
				Name = model.Name,
				Sku = model.Sku,
				Price = model.Price,
				QuantityInStock = model.QuantityInStock
			}, cancellationToken);

			TempData["Success"] = "Product updated successfully.";
			return RedirectToAction(nameof(Index));
		}
		catch (System.Exception ex)
		{
			ModelState.AddModelError(string.Empty, ex.Message);
			return View(model);
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(
			int id,
			CancellationToken cancellationToken)
	{
		try
		{
			await _productService.DeleteAsync(id, cancellationToken);
			TempData["Success"] = "Product deleted successfully.";
		}
		catch (System.Exception ex)
		{
			TempData["Error"] = ex.Message;
		}

		return RedirectToAction(nameof(Index));
	}

	[HttpGet]
	public async Task<IActionResult> Search(string term, CancellationToken cancellationToken)
	{
		var products = await _productService.SearchAsync(term, cancellationToken);
		return PartialView("_ProductTableRows", products);
	}
}