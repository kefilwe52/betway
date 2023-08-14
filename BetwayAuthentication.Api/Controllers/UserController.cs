using BetwayAuthentication.DAL.Entities;
using BetwayAuthentication.DAL.Models;
using BetwayAuthentication.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetwayAuthentication.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;
		public UserController(IUserService userService, IConfiguration configuration)
		{
			_userService = userService;
			_configuration = configuration;
		}

		[HttpPost("signup")]
		public IActionResult SignUp(User user)
		{
			try
			{
				var newUser = _userService.SignUp(user);
				return Ok(newUser);
			}
			catch (ValidationException ex)
			{
				return BadRequest(new { ex.Message });
			}
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public IActionResult Login(Login loginModel)
		{
			try
			{
				var user = _userService.Login(loginModel);
				if (user == null) return Unauthorized();

				return Ok(new
				{
					Token = GenerateTokenForUser(user.Id.ToString()),
					UserInfo = new
					{
						user.Id,
						user.Email,
						user.FirstName,
						user.LastName
					}
				});
			}
			catch (ValidationException ex)
			{
				return BadRequest(new { ex.Message });
			}
		}

		private string GenerateTokenForUser(string userId)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var keyString = _configuration["JwtConfig:Secret"];
			var key = Encoding.ASCII.GetBytes(keyString);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		[HttpGet("forgotpassword")]
		public IActionResult ForgotPassword(string email)
		{
			var user = _userService.ForgotPassword(email);
			if (user == null) return NotFound();

			return Ok(new { UserName = $"{user.FirstName} {user.LastName}", user.Email });
		}
	}
}
