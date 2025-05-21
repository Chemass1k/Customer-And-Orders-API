using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services.Interfaces;
using Customer_And_Orders.DAL.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (result != null)
            {
                var response = new ApiResponse<IEnumerable<User>>(true, "List of clients is recieved!", result);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>(false, "No clients found", null);
                return NotFound(response);
            }
        }

        [HttpGet("get-clients-orders/{id}")]
        public async Task<IActionResult> GetClientsOrdersAsync(int id)
        {
            var result = await _service.GetClientsOrdersAsync(id);
            if (result != null)
            {
                var response = new ApiResponse<IEnumerable<Order>>(true, $"Orders from client with id {id} are recieved!", result);
                return Ok(response);
            }
            else
            {
                var response = new ApiResponse<string>(false, $"Orders for client with id {id} weren't found!", null);
                return NotFound(response);
            }
        }

        [HttpPut("update-clients-data")]
        public async Task<IActionResult> UpdateClientsData(UpdateUserDTO user)
        {
            var result = await _service.ChangeClientData(user);
            return Ok(result);
        }

        [HttpDelete("delete-client")]
        public async Task<IActionResult> DeleteClient([FromQuery]int clientId)
        {
            var result  = await _service.DeleteClient(clientId);
            return Ok(result);
        }

    }
}
