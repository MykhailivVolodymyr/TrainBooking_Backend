using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrainBooking.Application.DTOs;
using TrainBooking.Application.Servises;
using TrainBooking.Infrastructure.Providers;

namespace TrainBooking.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtOptions _jwtOptions;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, IOptions<JwtOptions> options)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = options.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            try
            {
                string token =  await _userService.AddAsync(userDto);
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("Jwt-token", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(_jwtOptions.ExpiresHours)
                });

                return StatusCode(201, "Користувача успішно зареєстровано");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Внутрішня помилка сервера" });
            }
        }
 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var loginResult = await _userService.Login(userLoginDto);
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("Jwt-token", loginResult.Token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(_jwtOptions.ExpiresHours)
                });


                return Ok(new
                {
                    message = "Log in successfully",
                    role = loginResult.Role,
                    fullName = loginResult.FullName
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Видалення куки з токеном
            Response.Cookies.Delete("Jwt-token");
            
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
