using Castle.Core.Logging;
using Customer_And_Orders.BAL.Models;
using Customer_And_Orders.BAL.Services;
using Customer_And_Orders.DAL.Data.Entities;
using Customer_And_Orders.DAL.Repositories.Intrefaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_And_Orders.Test.Services
{
    public class AuthServiceTest
    {
        private readonly Mock<IAuthRepository> _repositoryMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private AuthService _authService;

        public AuthServiceTest()
        {
            _repositoryMock = new Mock<IAuthRepository>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _authService = new AuthService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnTrue_WhenUsernameIsNotTaken()
        {
            var requestBAL = new BAL.Models.CreateUserDTO
            {
                Email = "1234",
                Password = "password"
            };

            var requestDAL = new User
            {
                Username = requestBAL.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(requestBAL.Password),
                Email = requestBAL.Email,
                Role = "Client",
                RefreshToken = null,
                RefreshExpiry = DateTime.Now,
                Id = 1
            };

            _repositoryMock
                .Setup(r => r.RegistrateAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            var result = await _authService.RegistrateAsync(requestBAL);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Login_ShouldReturnTokens_WhenUsernameAndPasswordCorrect()
        {
            var user = new LoginUserDTO
            {
                Username = "1234",
                Password = "12345!"
            };

            _repositoryMock
                .Setup(r => r.LoginAsync(user.Username, user.Password))
                .ReturnsAsync(("access-token", "refresh-token"));


            var result = await _authService.LoginAsync(user);
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnTokens_WhenValid()
        {
            var refreshToken = "valid-refresh-token";
            var expectedAccess = "new-access-token";
            var expectedRefresh = "new-refresh-token";

            _repositoryMock
                .Setup(r => r.RefreshTokenAsync(refreshToken))
                .ReturnsAsync((expectedAccess, expectedRefresh));

            var result = await _authService.RefreshTokensAsync(refreshToken);

            result.AccessToken.Should().Be(expectedAccess);
            result.RefreshToken.Should().Be(expectedRefresh);
        }
    }
}
