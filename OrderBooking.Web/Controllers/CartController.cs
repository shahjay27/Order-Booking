using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;

namespace OrderBooking.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCart(userId);
            if(response!=null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }

        public async Task<IActionResult> Remove(int cartDetailId)
        {
            ResponseDto? response = await this._cartService.CartRemove(cartDetailId);

            if(response!=null && response.IsSuccess)
            {
                TempData["success"] = "Idem removed from cart";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await this._cartService.ApplyCoupon(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Applied!";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await this._cartService.RemoveCoupon(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Removed!";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(x=>x.Type==JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value;

            ResponseDto? response = await this._cartService.EmailCart(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent sortly!";
                return RedirectToAction(nameof(CartIndex));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(CartIndex));
        }
    }
}
