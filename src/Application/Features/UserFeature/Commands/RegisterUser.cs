using FluentValidation;
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

	public RegisterUserHandler(IAuthorizationService authorizationService, IAvailabilityService availabilityService, IUserRepository userRepository, IPasswordManager passwordManager)
	{
		_authorizationService = authorizationService;
		_availabilityService = availabilityService;
		_userRepository = userRepository;
		_passwordManager = passwordManager;
	}

	public async Task<Unit> Handle(RegisterUser request, CancellationToken cancellationToken)
	{
		var email = request.Email;
		var userName = request.UserName;
		var password = request.Password;

		var available = await _availabilityService.CheckUser(email, userName);

		var hashedPassword = _passwordManager.Secure(password);
		var user = new User(email, hashedPassword, userName);

		if (available)
		{
			await _userRepository.AddAsync(user);
			await _authorizationService.AddUserToRoleAsync(user.Id, "User");
		}

		return Unit.Value;
	}
}

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
	public RegisterUserValidator()
	{
		RuleFor(x => x.Email)
			.NotNull()
			.MaximumLength(128)
			.EmailAddress();
		RuleFor(x => x.UserName)
			.NotNull()
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