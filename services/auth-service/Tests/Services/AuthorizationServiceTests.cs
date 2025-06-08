using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.DTOs.Auth;
using AuthService.Models;
using AuthService.Services.Implementations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using SharedLibrary.Security.JWT;
using SharedLibrary.Security.Password;
using AuthService.Services.Interfaces; // Add this if IJwtTokenService and IPasswordHasher are here
using AuthService.Data; // Add this if AuthDbContext is here
using Xunit;

namespace AuthService.Tests.Services
{
    public class AuthorizationServiceTests
    {
        private const string AccessTokenValue = "access";

        private readonly Mock<AuthDbContext> _dbContextMock = new(MockBehavior.Strict);
        private readonly Mock<IJwtTokenService> _jwtServiceMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IValidator<LoginRequest>> _loginValidatorMock = new();
        private readonly Mock<IValidator<RefreshTokenRequest>> _refreshValidatorMock = new();

        private AuthorizationService CreateService(AuthDbContext? dbContext = null)
        {
            return new AuthorizationService(
                dbContext ?? _dbContextMock.Object,
                _jwtServiceMock.Object,
                _passwordHasherMock.Object,
                _loginValidatorMock.Object,
                _refreshValidatorMock.Object
            );
        }


        [Fact]
        public async Task LoginAsync_Should_Throw_When_Validation_Fails()
        {
            // Arrange
            var request = new LoginRequest();
            _loginValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = { new FluentValidation.Results.ValidationFailure("Username", "Required") }
                });

            var service = CreateService();
            

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => service.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var request = new LoginRequest { Username = "notfound", Password = "pw" };
            _loginValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var dbContext = new Mock<AuthDbContext>();
            dbContext.Setup(x => x.Users)
                .Returns(DbSetMock.Empty<User>());
            var service = CreateService(dbContext.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_Should_Return_Token_When_Success()
        {
            // Arrange
            var request = new LoginRequest { Username = "user", Password = "pw" };
            var user = new User
            {
                Id = 1,
                Username = "user",
                PasswordHash = "hashed",
                CompanyId = 2,
                UserRoles = { new UserRole { RoleId = 3 } },
                RefreshTokens = { }
            };
            _loginValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var users = new List<User> { user }.AsQueryable();
            var dbSet = DbSetMock.Create(users);
            var dbContext = new Mock<AuthDbContext>();
            dbContext.Setup(x => x.Users).Returns(dbSet);
            _passwordHasherMock.Setup(h => h.Verify("pw", "hashed")).Returns(true);
            _jwtServiceMock.Setup(j => j.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>())).Returns(AccessTokenValue);
            _jwtServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh");

            var service = CreateService(dbContext.Object);

            // Act
            var result = await service.LoginAsync(request);

            // Assert
            Assert.Equal(AccessTokenValue, result.AccessToken);
            Assert.Equal("refresh", result.RefreshToken);
            Assert.True(result.ExpiresAt > DateTime.UtcNow);
        }

        [Fact]
        public async Task RefreshAsync_Should_Throw_When_Validation_Fails()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "invalid" };
            _refreshValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = { new FluentValidation.Results.ValidationFailure("RefreshToken", "Required") }
                });

            var service = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => service.RefreshAsync(request));
        }

        [Fact]
        public async Task RefreshAsync_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "notfound" };
            _refreshValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var dbContext = new Mock<AuthDbContext>();
            dbContext.Setup(x => x.Users)
                .Returns(DbSetMock.Empty<User>());
            var service = CreateService(dbContext.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.RefreshAsync(request));
        }

        [Fact]
        public async Task RefreshAsync_Should_Throw_When_Token_Expired()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "expiredtoken" };
            var user = new User
            {
                Id = 1,
                Username = "user",
                RefreshTokens = { new RefreshToken { Token = "expiredtoken", ExpiresAt = DateTime.UtcNow.AddMinutes(-1) } }
            };
            _refreshValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var users = new List<User> { user }.AsQueryable();
            var dbSet = DbSetMock.Create(users);
            var dbContext = new Mock<AuthDbContext>();
            dbContext.Setup(x => x.Users).Returns(dbSet);

            var service = CreateService(dbContext.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.RefreshAsync(request));
        }

        [Fact]
        public async Task RefreshAsync_Should_Return_NewToken_When_Success()
        {
            // Arrange
            var request = new RefreshTokenRequest { RefreshToken = "validtoken" };
            var user = new User
            {
                Id = 1,
                Username = "user",
                CompanyId = 2,
                UserRoles = { new UserRole { RoleId = 3 } },
                RefreshTokens = { new RefreshToken { Token = "validtoken", ExpiresAt = DateTime.UtcNow.AddMinutes(10) } }
            };
            _refreshValidatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var users = new List<User> { user }.AsQueryable();
            var dbSet = DbSetMock.Create(users);
            var dbContext = new Mock<AuthDbContext>();
            dbContext.Setup(x => x.Users).Returns(dbSet);
            _jwtServiceMock.Setup(j => j.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>())).Returns(AccessTokenValue);
            _jwtServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("newrefresh");

            var service = CreateService(dbContext.Object);

            // Act
            var result = await service.RefreshAsync(request);

            // Assert
            Assert.Equal(AccessTokenValue, result.AccessToken);
            Assert.Equal("newrefresh", result.RefreshToken);
            Assert.True(result.ExpiresAt > DateTime.UtcNow);
        }

        [Fact]
        public async Task LogoutAsync_Should_Remove_RefreshToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "user",
                RefreshTokens = { new RefreshToken { Token = "logouttoken", ExpiresAt = DateTime.UtcNow.AddMinutes(10) } }
            };
            var users = new List<User> { user }.AsQueryable();
            var dbSet = DbSetMock.Create(users);
            var dbContext = new Mock<AuthDbContext>();
            dbContext.Setup(x => x.Users).Returns(dbSet);

            var service = CreateService(dbContext.Object);

            // Act
            await service.LogoutAsync(user.Id);

            // Assert
            Assert.Empty(user.RefreshTokens);
        }
        // Helper untuk mock DbSet
       public static class DbSetMock
{
        public static DbSet<T> Create<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object; // ← Kembalikan DbSet<T> langsung, bukan Mock
        }

        public static DbSet<T> Empty<T>() where T : class => Create(new List<T>().AsQueryable());
    }
    

   

    
     
    }
}