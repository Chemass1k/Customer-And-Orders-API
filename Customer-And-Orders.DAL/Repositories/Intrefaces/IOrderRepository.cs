using Customer_And_Orders.DAL.Data.Entities;
using System.Threading.Tasks;

namespace Customer_And_Orders.DAL.Repositories.Intrefaces
{
    public interface IOrderRepository
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId, QueryParams? queryParams);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> CreateOrderAsync(Order order);
    }
}
