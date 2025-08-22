
using OrderBooking.Web.Models;

namespace OrderBooking.Web.Service.IService
{
    public interface ICartService
    {
        public Task<ResponseDto?> CartUpsert(CartDto cartDto);
        public Task<ResponseDto?> CartRemove(int cartDetailsId);
        public Task<ResponseDto?> GetCart(string userId);
        public Task<ResponseDto?> ApplyCoupon(CartDto cartDto);
        public Task<ResponseDto?> RemoveCoupon(CartDto cartDto);
    }
}
