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
            if (result)
                return Ok(user);
            else
                return BadRequest();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
        {
            var (token, refreshToken) = await _service.LoginAsync(loginUser);
            var tokens = new {token, refreshToken};
            return Ok(tokens);
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Works");
        }
    }
}
