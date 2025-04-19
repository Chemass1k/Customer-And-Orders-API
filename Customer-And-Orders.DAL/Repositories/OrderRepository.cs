using Customer_And_Orders.DAL.Data;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Customer_And_Orders.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _log;

        public OrderRepository(AppDbContext context, ILogger<OrderRepository> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _log.LogInformation($"Deleting order with ID {id} from database");
                var order = await _context.Order.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    _log.LogWarning($"Order with id {id} isn't exist in database");
                    return false;
                }
                var result = _context.Order.Remove(order);
                await _context.SaveChangesAsync();
                _log.LogInformation("Order deleted from database!");
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Order deleting from database finished with error. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId, QueryParams? queryParams)
        {
            try
            {
                _log.LogInformation($"Getting all user's: {userId} orders from database");
                var query = _context.Order.AsQueryable();

                query = query.Where(o => o.UserId == userId);

                if (!string.IsNullOrWhiteSpace(queryParams?.Status))
                    query = query.Where(o => o.Status == queryParams.Status);

                if(!string.IsNullOrEmpty(queryParams?.Search))
                    query = query.Where(o => o.Title == queryParams.Search);
               
                IOrderedQueryable<Order> orderedQuery;

                if (queryParams?.SortByDate == true)
                    query = query.OrderByDescending(o => o.CreatedAt).ThenByDescending(o => o.Id);
                else if (queryParams?.Sort?.ToLower() == "desc")
                    query = query.OrderByDescending(o => o.Id);
                else
                    query = query.OrderBy(o => o.Id);


                int skip = (queryParams.Page - 1) * queryParams.PageSize;
                var pagedOrders = await query
                    .Skip(skip)
                    .Take(queryParams.PageSize)
                    .ToListAsync();

                _log.LogInformation($"Recived all orders for user {userId}");
                
                return pagedOrders;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error getting all user's: {userId} orders from database. Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                _log.LogInformation($"Looking for order with id {id} in database");
                var order = await _context.Order.FirstOrDefaultAsync(u => u.Id == id);
                if(order == null)
                {
                    _log.LogWarning($"Order with Id: {id} is missing in database");
                    return null;
                }
                _log.LogInformation($"Recieved order with Id: {id}");
                return order;
            }
            catch (Exception ex)
            {
                _log.LogInformation($"Getting order with Id {id} finished with error. Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                _log.LogInformation($"Updating order {order.Id}");
                 _context.Order.Update(order);
                _context.Entry(order).Property(x => x.CreatedAt).IsModified = false;
                _context.Entry(order).Property(x => x.UserId).IsModified = false;
                await _context.SaveChangesAsync();
                _log.LogInformation($"Order with Id {order.Id} updated!");
                return order;

            }
            catch (Exception ex)
            {
                _log.LogError($"Updating order in database finished with error. Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                _log.LogInformation("Adding order to database!");
                await _context.Order.AddAsync(order);
                await _context.SaveChangesAsync();
                var result = await _context.Order.OrderBy(i => i.Id).LastAsync();
                _log.LogInformation($"Order added to database successfuly with id {result.Id}");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError("Error adding order to database!");
                return null;
            }
        }
    }
}
