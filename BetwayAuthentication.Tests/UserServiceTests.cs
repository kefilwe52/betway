using BetwayAuthentication.DAL.Entities;
using BetwayAuthentication.DAL.Models;
using BetwayAuthentication.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BetwayAuthentication.Tests
{
	public class UserServiceTests
	{
		private readonly Mock<DataContext> _mockContext;
		private readonly UserService _userService;
		private List<User> _users;

		public UserServiceTests()
		{
			_mockContext = new Mock<DataContext>(new DbContextOptions<DataContext>());
			_users = new List<User>();

			var mockSet = new Mock<DbSet<User>>();
			mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_users.AsQueryable().Provider);
			mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_users.AsQueryable().Expression);
			mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_users.AsQueryable().ElementType);
			mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_users.GetEnumerator());
			mockSet.Setup(m => m.Add(It.IsAny<User>())).Callback<User>(_users.Add);

			_mockContext.Setup(m => m.Users).Returns(mockSet.Object);

			_userService = new UserService(_mockContext.Object);
		}

		[Fact]
		public void SignUp_ValidUser_SavesAndReturnsUser()
		{
			var user = new User
			{
				Email = "test@example.com",
				Password = "password123",
				FirstName = "Test",
				LastName = "User",
				MobileNumber = "1234567890"
			};

			var result = _userService.SignUp(user);

			Assert.Single(_users);
			Assert.Equal(user.Email, _users.First().Email);
			Assert.Equal(user.Email, result.Email);
		}

		[Fact]
		public void SignUp_InvalidUser_ThrowsValidationException()
		{
			var user = new User { Email = "test@", Password = "pass" };

			Assert.Throws<ValidationException>(() => _userService.SignUp(user));
		}

		[Fact]
		public void Login_ValidCredentials_ReturnsUser()
		{
			var existingUser = new User
			{
				Email = "test@example.com",
				Password = _userService.HashPassword("password123")
			};
			_users.Add(existingUser);

			var loginModel = new Login { Email = "test@example.com", Password = "password123" };

			var result = _userService.Login(loginModel);

			Assert.NotNull(result);
			Assert.Equal(existingUser.Email, result.Email);
		}

		[Fact]
		public void Login_InvalidCredentials_ThrowsValidationException()
		{
			var loginModel = new Login { Email = "test@", Password = "pass" };

			Assert.Throws<ValidationException>(() => _userService.Login(loginModel));
		}

		[Fact]
		public void ForgotPassword_ValidEmail_ReturnsUser()
		{
			var existingUser = new User { Email = "test@example.com", Password = "hashedpassword" };
			_users.Add(existingUser);

			var result = _userService.ForgotPassword("test@example.com");

			Assert.NotNull(result);
			Assert.Equal(existingUser.Email, result.Email);
		}

		[Fact]
		public void ForgotPassword_InvalidEmail_ReturnsNull()
		{
			var result = _userService.ForgotPassword("nonexistent@example.com");
			Assert.Null(result);
		}
	}
}
