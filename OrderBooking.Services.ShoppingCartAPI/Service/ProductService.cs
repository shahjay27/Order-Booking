using Newtonsoft.Json;
using OrderBooking.Services.ShoppingCartAPI.Models.DTO;
using OrderBooking.Services.ShoppingCartAPI.Service.IService;
using System.Text.Json.Serialization;

namespace OrderBooking.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _http;

        public ProductService(IHttpClientFactory http)
        {
            this._http = http;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var client = this._http.CreateClient("Product");
                var response = await client.GetAsync($"/api/productapi");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent.ToString());
                if (resp.IsSuccess)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
                }
                return new List<ProductDto>();
            }
            catch (Exception ex)
            {
                return new List<ProductDto>();
            };
        }
    }
}
