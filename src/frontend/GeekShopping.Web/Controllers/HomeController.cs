using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace GeekShopping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
            var products = await _productService.FindAllProductsAsync(token);
            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
            var model = await _productService.FindProductByIdAsync(id, token);
            return View(model);
        }
        
        [Authorize]
        [ActionName("Details")]
        [HttpPost]
        public async Task<IActionResult> DetailsPost(ProductViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
            CartViewModel cart = new()
            {
                CartHeader = new()
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value ?? throw new ArgumentException("User type sub não encontrado."),

                }
            };

            CartDetailViewModel detail = new()
            {
                Count = model.Count,
                ProductId = model.Id,
                Product = await _productService.FindProductByIdAsync(model.Id, token),
            };

            List<CartDetailViewModel> cartDeails = new List<CartDetailViewModel>();

            cartDeails.Add(detail);
            cart.CartDetails = cartDeails;

            var response = await _cartService.AddItemToCart(cart, token);

            if (response != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index)); 
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

    }
}