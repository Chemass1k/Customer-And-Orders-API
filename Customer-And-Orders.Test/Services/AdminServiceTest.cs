using Customer_And_Orders.DAL.Data.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using FluentAssertions;

namespace Customer_And_Orders.Test.Services
{
    public class AdminServiceTest
    {
        private readonly Mock<IAdminRepository> _repositoryMock;
        private readonly Mock<ILogger<AdminService>> _loggerMock;
        private readonly AdminService _adminService;

        public AdminServiceTest()
        {
            _repositoryMock = new Mock<IAdminRepository>();
            _loggerMock = new Mock<ILogger<AdminService>>();
            _adminService = new AdminService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ChangeClientData_ShouldChangeClientData_WhenUserExist()
        {
            var repoClientBAL = new UpdateUserDTO
            {
                Id = 10,
                Email = "Test",
                Role = "Client",
                Username = "Test Client"
            };

            var repoClientDAL = new User
            {
                Id = 10,
                Email = "Test",
                Role = "Client",
                Username = "Test Client"
            };

            var updatedClientBAL = new UpdateUserDTO
            {
                Id = 10,
                Email = "Test",
                Role = "Admin",
                Username = "Updated Client"
            };

            var updatedClientDAL = new User
            {
                Id = 10,
                Email = "Test",
                Role = "Admin",
                Username = "Updated Client"
            };

            _repositoryMock.Setup(c => c.ChangeClientData(It.Is<User>(u =>
            u.Id == repoClientDAL.Id &&
            u.Email == repoClientDAL.Email &&
            u.Role == repoClientDAL.Role &&
            u.Username == repoClientDAL.Username))).ReturnsAsync(updatedClientDAL);

            var result = await _adminService.ChangeClientData(repoClientBAL);
            result.Should().BeEquivalentTo(updatedClientBAL);
        }
        [Fact]
        public async Task DeleteClient_ShouldDeleteClient_WhenClientExist()
        {
            int id = 10;
            _repositoryMock
                .Setup(c => c.DeleteClient(id))
                .ReturnsAsync(true);

            var result = await _adminService.DeleteClient(id);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetClientsAsync_ShouldReturnClientsList_WhenClientsExist()
        {
            var client1 = new User { Id = 1, Email = "Test1", Role = "Client", Username = "Test1"};
            var client2 = new User { Id = 2, Email = "Test2", Role = "Client", Username = "Test2" };
            var clients = new List<User> { client1, client2 };

            _repositoryMock
                .Setup(c => c.GetClientsAsync())
                .ReturnsAsync(clients);

            var result = await _adminService.GetClientsAsync();

            result.Should().BeEquivalentTo(clients);
            result.Should().HaveCount(2);
            
        }
        [Fact]
        public async Task GetClientsOrdersAsync_ShouldReturnOrders_WhenClientExist()
        {
            int userId = 10;
            var order1 = new Order { Id = 1, Title = "Test1", UserId = userId };
            var order2 = new Order { Id = 2, Title = "Test2", UserId = userId };
            var orders = new List<Order> { order1, order2 };

            _repositoryMock
                .Setup(o => o.GetClientsOrdersAsync(userId))
                .ReturnsAsync(orders);

            var result = await _adminService.GetClientsOrdersAsync(userId);

            result.Should().BeEquivalentTo(orders);
            result.Should().HaveCount(2);
        }
    }
}
