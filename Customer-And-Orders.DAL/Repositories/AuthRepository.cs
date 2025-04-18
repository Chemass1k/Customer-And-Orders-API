using Customer_And_Orders.DAL.Data;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customer_And_Orders.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthRepository> _log;

        public AuthRepository(AppDbContext coontext, ILogger<AuthRepository> log)
        {
            _context = coontext;
            _log = log;
        }

        public async Task<bool> RegistrateAsync(User user)
        {
            try
            {
                _log.LogInformation($"Addint {user.Username} to database");
                if(user == null)
                {
                    _log.LogWarning($"Cannot add {user.Username} to database. Information missing");
                    return false;
                }

                if(await _context.User.AnyAsync(u => u.Username == user.Username))
                {
                    _log.LogWarning($"User with username {user.Username} is exist in database");
                    return false;
                }

                await _context.User.AddAsync( user );
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error adding {user.Username} to database. Error: {ex.Message}");
                return false;
            }
        }
    }
}
