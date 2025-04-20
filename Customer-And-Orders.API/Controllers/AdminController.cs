using Customer_And_Orders.BAL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer_And_Orders.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet("get-clients")]
        public async Task<IActionResult> GetClientsAsync()
        {
            var result = await _service.GetClientsAsync();
            return Ok(result);
        }

        [HttpGet("get-clients-orders/{id}")]
        public async Task<IActionResult> GetClientsOrdersAsync([FromQuery] int id)
        {
            var result = await _service.GetClientsOrdersAsync(id);
            return Ok(result);
        }

    }
}
