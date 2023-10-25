using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.UserFeature.Commands;

public record RegisterUser(string Email, string UserName, string Password, string ConfirmPassword) : IRequest<Unit>;

internal sealed class RegisterUserHandler : IRequestHandler<RegisterUser, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IAvailabilityService _availabilityService;
	private readonly IPasswordManager _passwordManager;
	private readonly IUserRepository _userRepository;

	public RegisterUserHandler(IAuthorizationService authorizationService, IUserRepository userRepository, IPasswordManager passwordManager, IAvailabilityService availabilityService)
	{
		_authorizationService = authorizationService;
		_userRepository = userRepository;
		_passwordManager = passwordManager;
		_availabilityService = availabilityService;
	}

	public async Task<Unit> Handle(RegisterUser request, CancellationToken cancellationToken)
	{
		var email = request.Email;
		var userName = request.UserName;
		var password = request.Password;

		var available = await _availabilityService.CheckUser(email, userName);

		if (available) throw new InvalidUserCredentials();

		var hashedPassword = _passwordManager.Secure(password);
		var user = new User(email, hashedPassword, userName);

		await _userRepository.AddAsync(user);
		await _authorizationService.AddUserToRoleAsync(user.Id, "User");

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