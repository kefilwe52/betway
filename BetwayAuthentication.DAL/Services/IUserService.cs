using BetwayAuthentication.DAL.Entities;
using BetwayAuthentication.DAL.Models;

namespace BetwayAuthentication.DAL.Services
{
	public interface IUserService
	{
		User SignUp(User user);
		User Login(Login loginModel);
		User ForgotPassword(string email);
	}
}
