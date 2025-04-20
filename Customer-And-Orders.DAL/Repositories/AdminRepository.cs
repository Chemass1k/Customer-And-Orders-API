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

        public async Task<User> ChangeClientData(User data)
        {
            try
            {
                _log.LogInformation($"Changing client's {data.Id} data in database");
                _context.User.Update(data);
                if (data.Email.IsNullOrEmpty())
                    _context.Entry(data).Property(x => x.Email).IsModified = false;
                if(data.Role.IsNullOrEmpty())
                    _context.Entry(data).Property(x => x.Role).IsModified = false;
                if(data.Username.IsNullOrEmpty())
                    _context.Entry(data).Property(x => x.Username).IsModified = false;

                _context.Entry(data).Property(x => x.PasswordHash).IsModified = false;
                _context.Entry(data).Property(x => x.RefreshToken).IsModified = false;
                _context.Entry(data).Property(x => x.RefreshExpiry).IsModified = false;
                await _context.SaveChangesAsync();
                var result = await _context.User.FirstOrDefaultAsync(u => u.Id == data.Id);
                _log.LogInformation("User's data updated!");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error updating user's data in database! Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteClient(int clientId)
        {
            try
            {
                _log.LogInformation($"Deleting client {clientId} from database");
                var client = await _context.User.FirstOrDefaultAsync(u => u.Id == clientId);
                if(client == null)
                {
                    _log.LogWarning($"Client with id {clientId} wasn't found");
                    return false;
                }
                _context.User.Remove(client);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error deleting client with id {clientId}. Error: {ex.Message}");
                return false;
            }
        }
    }
}
