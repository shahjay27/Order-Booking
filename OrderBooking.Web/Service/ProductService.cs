using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using OrderBooking.Web.Utility;

namespace OrderBooking.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto Product)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = Product,
                Url = StaticDetails.ProductAPIBase + "/api/Productapi"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int ProductId)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.DELETE,
                Url = StaticDetails.ProductAPIBase + "/api/Productapi/" + ProductId
            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.ProductAPIBase + "/api/Productapi"
            });
        }

        public async Task<ResponseDto?> GetProductAsync(string ProductCode)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.ProductAPIBase + "/api/Productapi/GetByCode/" + ProductCode
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int ProductId)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.GET,
                Url = StaticDetails.ProductAPIBase + "/api/Productapi/" + ProductId
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto Product)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.PUT,
                Data = Product,
                Url = StaticDetails.ProductAPIBase + "/api/Productapi"
            });
        }
    }
}
