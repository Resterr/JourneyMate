using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JourneyMate.Infrastructure.Security;
internal sealed class PasswordManager : IPasswordManager
{
	private const User _user = default;
	private readonly IPasswordHasher<User> _passwordHasher;

	public PasswordManager(IPasswordHasher<User> passwordHasher)
	{
		_passwordHasher = passwordHasher;
	}

	public string Secure(string password) => _passwordHasher.HashPassword(_user, password);

	public bool Validate(string password, string securedPassword)
		=> _passwordHasher.VerifyHashedPassword(_user, securedPassword, password) == PasswordVerificationResult.Success;
}
