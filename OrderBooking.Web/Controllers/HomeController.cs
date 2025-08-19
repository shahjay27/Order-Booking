using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using System.Diagnostics;

namespace OrderBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IProductService _ProductService;

		public HomeController(ILogger<HomeController> logger, IProductService ProductService)
        {
            _logger = logger;
			_ProductService = ProductService;
		}

        public async Task<IActionResult> Index()
        {
			List<ProductDto> Products = new();

			var response = await this._ProductService.GetAllProductAsync();

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

            var response = await this._ProductService.GetProductByIdAsync(productId);

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