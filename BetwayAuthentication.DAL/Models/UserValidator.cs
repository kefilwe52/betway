﻿using BetwayAuthentication.DAL.Entities;
using FluentValidation;

namespace BetwayAuthentication.DAL.Models
{
	public class UserValidator : AbstractValidator<User>
	{
		public UserValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");

			RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");

			RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
								 .EmailAddress().WithMessage("Invalid email format.");

			RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
									.MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

			RuleFor(x => x.MobileNumber).NotEmpty().WithMessage("Mobile number is required.")
										.Matches(@"^\d{10}$").WithMessage("Invalid mobile number.");
		}
	}
}
