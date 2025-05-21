using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer_And_Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO user)
        {
            var result = await _service.RegistrateAsync(user);
            var okResponse = new ApiResponse<bool>(true, "Client registered!", result);
            var badResponse = new ApiResponse<bool>(false, "Client is not registered", result);
            if (result)
                return Ok(okResponse);
            else
                return BadRequest(badResponse);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
        {
            var (token, refreshToken) = await _service.LoginAsync(loginUser);
            var tokens = new { token, refreshToken };
            if (tokens != null)
            {
                var response = new ApiResponse<object>(true, "Tokens are recieved!", tokens);
                return Ok(tokens);
            }
            else
            {
                var response = new ApiResponse<string>(false, "Tokens aren't recieved!", null);
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokens([FromBody] string refreshToken)
        {
            var tokens = await _service.RefreshTokensAsync(refreshToken);
            if (tokens != (null, null))
            {
                var response = new ApiResponse<object>(true, $"Token: {tokens.AccessToken}, refreshToken: {tokens.RefreshToken}", tokens);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>(false, "Tokens aren't refreshed!", null);
                return BadRequest(response);
            }

        }
    }
}
