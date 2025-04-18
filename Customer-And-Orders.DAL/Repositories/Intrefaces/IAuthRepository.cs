using Customer_And_Orders.DAL.Data.Entities;

namespace Customer_And_Orders.DAL.Repositories.Intrefaces
{
    public interface IAuthRepository
    {
        Task<bool> RegistrateAsync(User user);
    }
}
