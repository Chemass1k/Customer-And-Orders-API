using Customer_And_Orders.DAL.Data;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Customer_And_Orders.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthRepository> _log;
        private readonly IConfiguration _config;

        public AuthRepository(AppDbContext coontext, ILogger<AuthRepository> log, IConfiguration config)
        {
            _context = coontext;
            _log = log;
            _config = config;
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password)
        {
            try
            {
                _log.LogInformation($"Trying to sign in {username}");
                var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    _log.LogWarning("Username or password is incorrect!");
                    return (null, null);
                }
                var tokens = CreateTokens(user);
                user.RefreshToken = tokens.RefreshToken;
                user.RefreshExpiry = DateTime.UtcNow.AddDays(7);

                await _context.SaveChangesAsync();
                _log.LogInformation($"User {username} signed in successfuly");
                return(tokens.AccessToken, tokens.RefreshToken);

            }
            catch (Exception ex)
            {
                _log.LogError($"Cannot to login {username}. Error: {ex.Message}");
                return (null, null);
            }
        }

        public async Task<bool> RegistrateAsync(User user)
        {
            try
            {
                _log.LogInformation($"Addint {user.Username} to database");
                if (user == null)
                {
                    _log.LogWarning($"Cannot add {user.Username} to database. Information missing");
                    return false;
                }

                if (await _context.User.AnyAsync(u => u.Username == user.Username))
                {
                    _log.LogWarning($"User with username {user.Username} is exist in database");
                    return false;
                }

                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error adding {user.Username} to database. Error: {ex.Message}");
                return false;
            }
        }

        public (string AccessToken, string RefreshToken) CreateTokens(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            return (accessToken, refreshToken);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
