using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.DAL.Data.Entities;

namespace Customer_And_Orders.BAL.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetClientsAsync();
        Task<IEnumerable<Order>> GetClientsOrdersAsync(int clientId);
        Task<User> ChangeClientData(UpdateUserDTO data);
        Task<bool> DeleteClient(int clientId);
    }
}
