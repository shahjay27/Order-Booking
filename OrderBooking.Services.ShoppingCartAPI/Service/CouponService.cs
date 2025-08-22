using Newtonsoft.Json;
using OrderBooking.Services.ShoppingCartAPI.Models.DTO;
using OrderBooking.Services.ShoppingCartAPI.Service.IService;

namespace OrderBooking.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _http;

        public CouponService(IHttpClientFactory http)
        {
            this._http = http;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = this._http.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/couponapi/GetByCode/{couponCode}");
            var apicontent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apicontent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new();
        }
    }
}
