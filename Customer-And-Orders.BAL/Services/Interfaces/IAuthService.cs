using Customer_And_Orders.BAL.Models;

namespace Customer_And_Orders.BAL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegistrateAsync(CreateUserDTO user);
        Task<(string AccessToken, string RefreshToken)> LoginAsync(LoginUserDTO user);
    }
}
