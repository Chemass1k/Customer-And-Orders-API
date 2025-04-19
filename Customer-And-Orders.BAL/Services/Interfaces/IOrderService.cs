using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.DAL.Data.Entities;

namespace Customer_And_Orders.BAL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Order>> GetAllByUserIdAsync(int userId, QueryParams? queryParams);
        Task<Order> GetByIdAsync(int id);
        Task<Order> UpdateOrderAsync(UpdateOrderDTO order);
        Task<Order> CreateOrderAsync(CreateOrderDTO order);
    }
}
