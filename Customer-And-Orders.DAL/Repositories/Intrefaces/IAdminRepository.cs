using Customer_And_Orders.DAL.Data.Entities;

namespace Customer_And_Orders.DAL.Repositories.Intrefaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetClientsAsync();
        Task<IEnumerable<Order>> GetClientsOrdersAsync(int clientId);
        Task<User> ChangeClientData(User data);
        Task<bool> DeleteClient(int clientId);
    }
}
