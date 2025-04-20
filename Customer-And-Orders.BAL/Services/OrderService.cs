using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services.Interfaces;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.Extensions.Logging;

namespace Customer_And_Orders.BAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repos;
        private readonly ILogger<OrderService> _log;

        public OrderService(IOrderRepository repos, ILogger<OrderService> log)
        {
            _repos = repos;
            _log = log;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDTO order)
        {
            try
            {
                _log.LogInformation($"Creating order");
                var newOrder = new Order
                {
                    Title = order.Title,
                    Description = order.Description,
                    Status = order.Status,
                    CreatedAt = DateTime.Now,
                    UserId = order.UserId
                };
                var result = await _repos.CreateOrderAsync(newOrder);
                if (result == null)
                {
                    _log.LogWarning("Something went wrong creating order");
                    return new Order();
                }
                _log.LogInformation($"Order created");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error creating order. Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _log.LogInformation($"Deleting order with id {id}");
                var result = await _repos.DeleteAsync(id);
                if(!result)
                {
                    _log.LogWarning($"Something went wrong deleting order");
                    return false;
                }
                _log.LogInformation($"Order deleted!");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error deleting order. Error {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId, QueryParams? queryParams)
        {
            try
            {
                _log.LogInformation($"Getting all user's {userId} orders");
                var result = await _repos.GetAllByUserIdAsync(userId, queryParams);
                if(result == null)
                {
                    _log.LogWarning("No orders found!");
                    return new List<Order>();
                }
                _log.LogInformation($"Orders recieved!");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error trying to get all orders for user {userId}. Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                _log.LogInformation($"Trying to get order with id {id}");
                var result = await _repos.GetOrderByIdAsync(id);
                if(result == null)
                {
                    _log.LogWarning("Order is not found");
                    return new Order();
                }
                _log.LogInformation($"");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error trying to get order with id {id}");
                return null;
            }
        }

        public async Task<Order> UpdateOrderAsync(UpdateOrderDTO order)
        {
            try
            {
                _log.LogInformation($"Updating order");
                var newOrder = new Order
                {
                    Id = order.Id,
                    Title = order.Title,
                    Description = order.Description,
                    Status = order.Status
                };
                var result = await _repos.UpdateOrderAsync(newOrder);
                if(result == null)
                {
                    _log.LogWarning($"Something went wrong updating order {order.Id}");
                    return new Order();
                }
                _log.LogInformation($"Order {result.Id} updated!");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error during update the order. Error {ex.Message}");
                return null;
            }
        }
    }
}
