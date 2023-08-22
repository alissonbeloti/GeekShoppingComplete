using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;

namespace GeekShopping.Web.Controllers;


public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    [Authorize]
    public async Task<IActionResult> ProductIndex()
    {
        var products = await _productService.FindAllProductsAsync(string.Empty);
        return View(products);
    }


    [Authorize]
    public IActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProductCreate(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
            var response = await _productService.CreateProductAsync(model, token);
            if (response != null)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> ProductUpdate(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
        var product = await _productService.FindProductByIdAsync(id, token);
        if (product != null)
            return View(product);
        else
            return NotFound();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProductUpdate(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
            var response = await _productService.UpdateProductAsync(model, token);
            if (response != null)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
        var product = await _productService.FindProductByIdAsync(id, token);
        if (product != null)
            return View(product);
        else
            return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductViewModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
        var response = await _productService.DeleteProductAsync(model.Id, token);
        if (response)
        {
            return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }
}
