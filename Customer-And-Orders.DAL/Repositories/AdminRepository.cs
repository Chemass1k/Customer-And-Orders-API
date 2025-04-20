using Customer_And_Orders.DAL.Data;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Customer_And_Orders.DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminRepository> _log;

        public AdminRepository(AppDbContext context, ILogger<AdminRepository> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<IEnumerable<User>> GetClientsAsync()
        {
            try
            {
                _log.LogInformation("Getting all clients from database");
                var clients = await _context.User.Where(u => u.Role == "Client").ToListAsync();
                if (clients.IsNullOrEmpty())
                {
                    _log.LogWarning("No clients found!");
                    return clients;
                }
                _log.LogInformation($"Got clients from database");
                return clients;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error getting clients from database. Error: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetClientsOrdersAsync(int clientId)
        {
            try
            {
                _log.LogInformation($"Trying to get all client's {clientId} orders");
                var orders = await _context.Order.Where(u => u.UserId == clientId).ToListAsync();
                if (orders.IsNullOrEmpty())
                {
                    _log.LogWarning($"No orders found for client {clientId}!");
                    return orders;
                }
                _log.LogInformation($"Got orders for client {clientId} from database");
                return orders;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error getting orders for client {clientId} from database. Error: {ex.Message}");
                return null;
            }
        }
    }
}
