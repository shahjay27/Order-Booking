using OrderBooking.Services.ShoppingCartAPI.Models.DTO;

namespace OrderBooking.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
    }
}
