using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using OrderBooking.Web.Utility;

namespace OrderBooking.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            this._baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = registrationRequestDto,
                Url = StaticDetails.AuthAPIBase + "/api/authapi/assignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = loginRequestDto,
                Url = StaticDetails.AuthAPIBase + "/api/authapi/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = StaticDetails.ApiTypes.POST,
                Data = registrationRequestDto,
                Url = StaticDetails.AuthAPIBase + "/api/authapi/register"
            }, withBearer: false);
        }
    }
}
