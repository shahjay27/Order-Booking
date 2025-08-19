using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using OrderBooking.Web.Utility;

namespace OrderBooking.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = coupon,
                Url = StaticDetails.CouponAPIBase + "/api/couponapi"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int couponId)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.DELETE,
                Url = StaticDetails.CouponAPIBase + "/api/couponapi/" + couponId
            });
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.CouponAPIBase + "/api/couponapi"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.CouponAPIBase + "/api/couponapi/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.CouponAPIBase + "/api/couponapi/" + couponId
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.PUT,
                Data = coupon,
                Url = StaticDetails.CouponAPIBase + "/api/couponapi"
            });
        }
    }
}
