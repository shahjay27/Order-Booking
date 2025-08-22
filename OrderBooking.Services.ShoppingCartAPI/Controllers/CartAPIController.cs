using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderBooking.Services.ShoppingCartAPI.Data;
using OrderBooking.Services.ShoppingCartAPI.Models;
using OrderBooking.Services.ShoppingCartAPI.Models.DTO;
using OrderBooking.Services.ShoppingCartAPI.Service.IService;
using System.Reflection.PortableExecutable;

namespace OrderBooking.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartAPIController(IMapper mapper, AppDbContext db, IProductService productService, ICouponService couponService)
        {
            this._mapper = mapper;
            this._db = db;
            this._response = new ResponseDto();
            this._productService = productService;
            this._couponService = couponService;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await this._db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader header = this._mapper.Map<CartHeader>(cartDto.CartHeader);
                    this._db.CartHeaders.Add(header);
                    await this._db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = header.CartHeaderId;
                    CartDetails details = this._mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    this._db.CartDetails.Add(details);
                    await this._db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await this._db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        x => x.CartHeaderId == cartHeaderFromDb.CartHeaderId &&
                        x.ProductId == cartDto.CartDetails.First().ProductId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cart details
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        CartDetails details = this._mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        this._db.CartDetails.Add(details);
                        await this._db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        this._db.CartDetails.Update(this._mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await this._db.SaveChangesAsync();
                    }
                }
                this._response.Result = cartDto;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpPost("CartRemove")]
        public async Task<ResponseDto> CartRemove([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await this._db.CartDetails.FirstAsync(x => x.CartDetailsId == cartDetailsId);

                int totalItemsForCust = this._db.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();

                this._db.CartDetails.Remove(cartDetails);
                await this._db.SaveChangesAsync();

                if (totalItemsForCust == 1)
                {
                    CartHeader cartHeaderToRemove = await this._db.CartHeaders.FirstAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    this._db.CartHeaders.Remove(cartHeaderToRemove);
                    await this._db.SaveChangesAsync();
                }


                this._response.Result = true;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = this._mapper.Map<CartHeaderDto>(this._db.CartHeaders.First(x => x.UserId == userId))
                };
                cart.CartDetails = this._mapper.Map<IEnumerable<CartDetailsDto>>(this._db.CartDetails.Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId));

                var allProducts = await this._productService.GetProducts();

                foreach (var cartDetails in cart.CartDetails)
                {
                    cartDetails.Product = allProducts.FirstOrDefault(x => x.ProductId == cartDetails.ProductId);
                    cart.CartHeader.CartTotal += (cartDetails.Count * cartDetails.Product.Price);
                }

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    var couponDto = await this._couponService.GetCoupon(cart.CartHeader.CouponCode);

                    if (couponDto != null && cart.CartHeader.CartTotal >= couponDto.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= couponDto.DiscountAmount;
                        cart.CartHeader.Discount=couponDto.DiscountAmount;
                    }
                }

                this._response.Result = cart;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await this._db.CartHeaders.FirstAsync(x => x.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                this._db.CartHeaders.Update(cartFromDb);
                await this._db.SaveChangesAsync();
                this._response.Result = true;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await this._db.CartHeaders.FirstAsync(x => x.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = "";
                this._db.CartHeaders.Update(cartFromDb);
                await this._db.SaveChangesAsync();
                this._response.Result = true;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }
    }
}
