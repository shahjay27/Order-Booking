using OrderBooking.Web.Models;

namespace OrderBooking.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetProductAsync(string ProductCode);
        Task<ResponseDto?> GetProductByIdAsync(int ProductId);
        Task<ResponseDto?> CreateProductAsync(ProductDto Product);
        Task<ResponseDto?> UpdateProductAsync(ProductDto Product);
        Task<ResponseDto?> DeleteProductAsync(int ProductId);
    }
}
