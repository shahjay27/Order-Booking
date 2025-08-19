using AutoMapper;
using OrderBooking.Services.CouponAPI.Models;
using OrderBooking.Services.CouponAPI.Models.DTO;

namespace OrderBooking.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();               
            });

            return mappingConfig;
        }
    }
}
