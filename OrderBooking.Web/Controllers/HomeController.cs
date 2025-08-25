using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace OrderBooking.Web.Controllers
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

        public async Task<IActionResult> Index()
        {
			List<ProductDto> Products = new();

			var response = await this._productService.GetAllProductAsync();

			if (response != null && response.IsSuccess)
			{
				Products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(Products);
		}

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto Product = new();

            var response = await this._productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(Product);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,
            };

            List<CartDetailsDto> cartDetailsDtos=new List<CartDetailsDto> { cartDetails };
            cartDto.CartDetails = cartDetailsDtos;

            ResponseDto? response =  await this._cartService.CartUpsert(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Idem added to cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}