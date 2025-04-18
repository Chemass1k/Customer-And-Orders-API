using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services.Interfaces;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.Extensions.Logging;

namespace Customer_And_Orders.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repos;
        private readonly ILogger<AuthService> _log;

        public AuthService(IAuthRepository repos, ILogger<AuthService> log)
        {
            _repos = repos;
            _log = log;
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(LoginUserDTO user)
        {
            try
            {
                _log.LogInformation($"Signing in {user.Username}");
                var result = await _repos.LoginAsync(user.Username, user.Password);
                _log.LogInformation($"User {user.Username} signed in!");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Login failed! Error: {ex.Message}");
                return (null, null);
            }
        }

        public async Task<bool> RegistrateAsync(CreateUserDTO user)
        {
            try
            {
                _log.LogInformation($"Trying to sign up user: {user.Username}");
                var userInf = new User
                {
                    Username = user.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    Email = user.Email,
                    Role = "Client"
                };
                var result = await _repos.RegistrateAsync(userInf);
                if (result)
                    _log.LogInformation($"User {user.Username} signed up successfuly");
                else
                    _log.LogWarning($"Cannot create user with username {user.Username}");
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error creating a new user. Error: {ex.Message}");
                return false;
            }
        }
    
        public async Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken)
        {
            try
            {
                _log.LogInformation($"Refreshing token");
                return await _repos.RefreshTokenAsync(refreshToken);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error refreshing token. Error: {ex.Message}");
                return (null, null);
            }
        }
    }
}
