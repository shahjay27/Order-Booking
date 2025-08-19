using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderBooking.Services.ProductAPI.Data;
using OrderBooking.Services.ProductAPI.Models;
using OrderBooking.Services.ProductAPI.Models.DTO;


namespace OrderBooking.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public ProductAPIController(AppDbContext appDbContext, IMapper mapper)
        {
            this._db = appDbContext;
            this._response = new ResponseDto();
            this._mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var products = this._db.Products.ToList();
                var productDtoList = this._mapper.Map<IEnumerable<ProductDto>>(products);

                this._response.Result = productDtoList;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }


            return this._response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get([FromRoute] int id)
        {
            try
            {
                var product = this._db.Products.First(x => x.ProductId == id);
                this._response.Result = this._mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public ResponseDto GetByName([FromRoute] string name)
        {
            try
            {
                var product = this._db.Products.First(x=>x.Name == name);
                this._response.Result=this._mapper.Map<ProductDto>(product);
            }
            catch(Exception ex)
            {
                this._response.IsSuccess=false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = this._mapper.Map<Product>(productDto);
                await this._db.Products.AddAsync(product);
                await this._db.SaveChangesAsync();

                this._response.Result = productDto;
            }
            catch(Exception e)
            {
                this._response.IsSuccess = false;
                this._response.Message = e.Message;
            }

            return this._response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = this._mapper.Map<Product>(productDto);
                this._db.Products.Update(product);
                await this._db.SaveChangesAsync();

                this._response.Result=productDto;
            }
            catch (Exception ex)
            {
                this._response.IsSuccess = false;
                this._response.Message = ex.Message;
            }

            return this._response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                var product = _db.Products.First(x => x.ProductId == id);
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
