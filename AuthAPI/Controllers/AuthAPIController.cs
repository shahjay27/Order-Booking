using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderBooking.Services.AuthAPI.Models.DTO;
using OrderBooking.Services.AuthAPI.Service.IService;

namespace OrderBooking.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            this._authService = authService;
            this._response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto req)
        {
            var errorMessage = await this._authService.Register(req);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                this._response.IsSuccess = false;
                this._response.Message= errorMessage;

                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            var loginResponse = await _authService.Login(req);
            if (loginResponse.User == null)
            {
                this._response.IsSuccess = false;
                this._response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }

            this._response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto req)
        {
            var assignRoleSuccess = await _authService.AssignRole(req.Email, req.Role.ToUpper());
            if (!assignRoleSuccess)
            {
                this._response.IsSuccess = false;
                this._response.Message = "Error Encountered";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
