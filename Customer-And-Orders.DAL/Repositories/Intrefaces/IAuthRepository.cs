using Customer_And_Orders.DAL.Data.Entities;

namespace Customer_And_Orders.DAL.Repositories.Intrefaces
{
    public interface IAuthRepository
    {
        Task<bool> RegistrateAsync(User user);
        Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password); 
    }
}
