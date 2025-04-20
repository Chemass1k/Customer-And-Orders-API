using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services.Interfaces;
using Customer_And_Orders.DAL.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Customer_And_Orders.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDTO order)
        {
            var result = await _service.CreateOrderAsync(order);
            if (result == null)
                return BadRequest();
            return Ok(order);
        }

        [HttpDelete("delete-order")]
        public async Task<IActionResult> DeleteOrderAsync([FromBody] int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update-order")]
        public async Task<IActionResult> UpdateOrderAsync([FromBody] UpdateOrderDTO order)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = role == "Admin"
                ? order.UserId
                : int.Parse(User.FindFirst("userId")!.Value);
            order.UserId = userId;
            var result = await _service.UpdateOrderAsync(order);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrdersByUserIdAsync([FromQuery] QueryParams? queryParams)
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            var result = await _service.GetAllByUserIdAsync(userId, queryParams);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
