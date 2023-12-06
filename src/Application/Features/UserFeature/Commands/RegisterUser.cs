using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Commands;

public record RegisterUser(string Email, string UserName, string Password, string ConfirmPassword) : IRequest<Unit>;

internal sealed class RegisterUserHandler : IRequestHandler<RegisterUser, Unit>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IPasswordManager _passwordManager;

	public RegisterUserHandler(IApplicationDbContext dbContext, IPasswordManager passwordManager)
	{
		_dbContext = dbContext;
		_passwordManager = passwordManager;
	}

	public async Task<Unit> Handle(RegisterUser request, CancellationToken cancellationToken)
	{
		var email = request.Email;
		var userName = request.UserName;
		var password = request.Password;

		if (await _dbContext.Users.AnyAsync(x => x.Email == email))
			throw new DataAlreadyTakenException(email, "Email");

		if (await _dbContext.Users.AnyAsync(x => x.UserName == userName))
			throw new DataAlreadyTakenException(userName, "Username");
		
		var hashedPassword = _passwordManager.Secure(password);
		var user = new User(email, hashedPassword, userName);
		
		var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == "User", cancellationToken);
		if (role != null)
			user.AddRole(role);
		else
			throw new RoleNotFoundException("User");

		await _dbContext.Users.AddAsync(user, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
	public RegisterUserValidator()
	{
		RuleFor(x => x.Email)
			.NotNull()
			.MinimumLength(6)
			.MaximumLength(128)
			.EmailAddress();
		RuleFor(x => x.UserName)
			.NotNull()
			.MinimumLength(3)
			.MaximumLength(128);
		RuleFor(x => x.Password)
			.NotNull()
			.MinimumLength(6)
			.MaximumLength(128);
		RuleFor(x => x.ConfirmPassword)
			.NotNull()
			.MinimumLength(6)
			.MaximumLength(128)
			.Equal(x => x.Password);
	}
}