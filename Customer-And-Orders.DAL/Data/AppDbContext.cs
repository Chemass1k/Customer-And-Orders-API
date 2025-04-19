
using Customer_And_Orders.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer_And_Orders.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}
