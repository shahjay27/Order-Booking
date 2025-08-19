using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;

namespace OrderBooking.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            this._ProductService = ProductService;
        }

        public async Task<IActionResult> ProductIndex()
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

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto Product)
        {
            if (ModelState.IsValid)
            {
                var response = await this._ProductService.CreateProductAsync(Product);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "success";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(Product);
        }

        public async Task<IActionResult> ProductUpdate(int ProductId)
        {
			ProductDto? Product = new();

			var response = await this._ProductService.GetProductByIdAsync(ProductId);

			if (response != null && response.IsSuccess)
			{
				Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(Product);
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return NotFound();
		}

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDto Product)
        {
            if (ModelState.IsValid)
            {
                var response = await this._ProductService.UpdateProductAsync(Product);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "success";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(Product);
        }

        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ProductDto? Product = new();

            var response = await this._ProductService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSuccess)
            {
                Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));      
                return View(Product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto Product)
        {
            var response = await this._ProductService.DeleteProductAsync(Product.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "success";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(Product);
        }
    }
}
