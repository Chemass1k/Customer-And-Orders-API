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
            {
                var response = new ApiResponse<string>(false, "A new order hasn't created!", null);
                return BadRequest(response);
            }
            else
            {
                var response = new ApiResponse<Order>(true, "A new order has created!", result);
                return Ok(response);
            }
        }

        [HttpDelete("delete-order")]
        public async Task<IActionResult> DeleteOrderAsync([FromBody] int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                var response = new ApiResponse<string>(false, "Order isn't deleted!", null);
                return BadRequest(response);

            }
            else
            {
                var response = new ApiResponse<bool>(true, "Order is deleted!", result)''
                return Ok(response);
            }
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
            {
                var response = new ApiResponse<string>(false, "Order isn't updated!", null);
                return BadRequest(response);
            }
            else
            {
                var response = new ApiResponse<Order>(true, "Order is updated!", result);
                return Ok(response);
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrdersByUserIdAsync([FromQuery] QueryParams? queryParams)
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);
            var result = await _service.GetAllByUserIdAsync(userId, queryParams);
            if (result == null)
            {
                var response = new ApiResponse<string>(false, $"Orders for client with id {userId} aren't found!", null);
                return NotFound(response);
            }
            else
            {
                var response = new ApiResponse<IEnumerable<Order>>(true, $"Orders for client with id {userId} are found!", result);
                return Ok(response);
            }
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                var response = new ApiResponse<string>(false, $"Order with id {id} isn't found!", null);
                return NotFound(response);
            }
            else
            {
                var response = new ApiResponse<Order>(true, $"Order with id {id} is found!", result);
                return Ok(response);
            }
        }
    }
}
