using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services.Interfaces;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.Extensions.Logging;

namespace Customer_And_Orders.BAL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repos;
        private readonly ILogger<AdminService> _log;

        public AdminService(IAdminRepository repos, ILogger<AdminService> log)
        {
            _repos = repos;
            _log = log;
        }

        public async Task<User> ChangeClientData(UpdateUserDTO data)
        {
            try
            {
                _log.LogInformation($"Changing clients {data.Id} data");
                var clientData = new User
                {
                    Id = data.Id,
                    Username = data.Username,
                    Email = data.Email,
                    Role = data.Role
                };
                var result = await _repos.ChangeClientData(clientData);
                _log.LogInformation($"Clients {data.Id} data changed!");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error changing client's data. Error {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteClient(int clientId)
        {
            try
            {
                _log.LogInformation($"Deleting client with id {clientId}");
                var result = await _repos.DeleteClient(clientId);
                _log.LogInformation($"Deleted client: {clientId}");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Failed to delete client: {clientId}");
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetClientsAsync()
        {
            try
            {
                _log.LogInformation("Request to get clients");
                var clients = await _repos.GetClientsAsync();
                _log.LogInformation("Clients recived successfuly");
                return clients;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error getting clients. Error {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetClientsOrdersAsync(int clientId)
        {
            try
            {
                _log.LogInformation($"Request to get client's {clientId} orders");
                var orders = await _repos.GetClientsOrdersAsync(clientId);
                _log.LogInformation($"Orders for client {clientId} recived!");
                return orders;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error getting orders for client {clientId}. Error {ex.Message}");
                return null;
            }
        }
    }
}
