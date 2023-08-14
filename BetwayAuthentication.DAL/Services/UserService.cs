using BetwayAuthentication.Api.Model;
using BetwayAuthentication.DAL.Entities;
using BetwayAuthentication.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace BetwayAuthentication.DAL.Services
{
	public class UserService : IUserService
	{
		private readonly DataContext _context;

		public UserService(DataContext context)
		{
			_context = context;
		}

		public User SignUp(User user)
		{
			var validator = new UserValidator();
			var results = validator.Validate(user);

			if (!results.IsValid)
			{
				var errorMessages = string.Join(", ", results.Errors.Select(e => e.ErrorMessage));
				throw new ValidationException(errorMessages);
			}

			user.Id = Guid.NewGuid();
			user.CreatedDate = DateTime.UtcNow;
			user.Password = HashPassword(user.Password);
			_context.Users.Add(user);
			_context.SaveChanges();
			return user;
		}

		public User Login(Login loginModel)
		{

			var validator = new LoginValidator();
			var results = validator.Validate(loginModel);

			if (!results.IsValid)
			{
				var errorMessages = string.Join(", ", results.Errors.Select(e => e.ErrorMessage));
				throw new ValidationException(errorMessages);
			}

			return _context.Users.FirstOrDefault(u => u.Email == loginModel.Email
			&& u.Password == HashPassword(loginModel.Password));
		}

		public User ForgotPassword(string email)
		{
			return _context.Users.FirstOrDefault(u => u.Email == email);
		}

		public string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}
	}
}
