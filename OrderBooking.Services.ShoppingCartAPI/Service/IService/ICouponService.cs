using OrderBooking.Services.ShoppingCartAPI.Models.DTO;

namespace OrderBooking.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCoupon(string couponCode);
    }
}
