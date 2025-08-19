using AutoMapper;
using OrderBooking.Services.ProductAPI.Models;
using OrderBooking.Services.ProductAPI.Models.DTO;

namespace OrderBooking.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
            });

            return mappingConfig;
        }
    }
}
