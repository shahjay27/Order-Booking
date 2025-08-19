using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OrderBooking.Web.Models;
using OrderBooking.Web.Service.IService;
using OrderBooking.Web.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrderBooking.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
            //return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            var response = await this._authService.LoginAsync(requestDto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response.Message;
                return View(requestDto);
            }
        }


        public async Task<IActionResult> Register()
        {
            var roles = new List<SelectListItem>()
            {
                new SelectListItem(StaticDetails.RoleAdmin, StaticDetails.RoleAdmin),
                new SelectListItem(StaticDetails.RoleCustomer, StaticDetails.RoleCustomer),
            };

            ViewBag.RoleList = roles;

            RegistrationRequestDto registrationRequestDto = new();
            return View(registrationRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto requestDto)
        {
            var response = await this._authService.RegisterAsync(requestDto);
            ResponseDto assignroleResponse;

            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(requestDto.Role))
                    requestDto.Role = StaticDetails.RoleCustomer;

                assignroleResponse = await this._authService.AssignRoleAsync(requestDto);

                if (assignroleResponse != null && assignroleResponse.IsSuccess)
                {
                    TempData["success"] = "Registration Successfull";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = response.Message;
            }

            var roles = new List<SelectListItem>()
            {
                new SelectListItem(StaticDetails.RoleAdmin, StaticDetails.RoleAdmin),
                new SelectListItem(StaticDetails.RoleCustomer, StaticDetails.RoleCustomer),
            };

            ViewBag.RoleList = roles;

            return View(requestDto);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            this._tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto req)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(req.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
