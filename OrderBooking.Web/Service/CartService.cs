using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using OrderBooking.Web.Utility;

namespace OrderBooking.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = cartDto,
                Url = StaticDetails.CartAPIBase + "/api/cartapi/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> CartRemove(int cartDetailsId)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = cartDetailsId,
                Url = StaticDetails.CartAPIBase + "/api/cartapi/CartRemove"
            });
        }

        public async Task<ResponseDto?> CartUpsert(CartDto cartDto)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = cartDto,
                Url = StaticDetails.CartAPIBase + "/api/cartapi/CartUpsert"
            });
        }

        public async Task<ResponseDto?> GetCart(string userId)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.CartAPIBase + $"/api/cartapi/GetCart/{userId}"
            });
        }

        public async Task<ResponseDto?> RemoveCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = cartDto,
                Url = StaticDetails.CouponAPIBase + "/api/cartapi/RemoveCoupon"
            });
        }
    }
}
