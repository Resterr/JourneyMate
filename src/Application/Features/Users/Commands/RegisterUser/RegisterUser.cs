using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Entities;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.Users.Commands.RegisterUser;
[AllowAnonymous]
public record RegisterUser(string Email, string UserName, string Password, string ConfirmPassword) : IRequest<Unit>;

internal sealed class RegisterUserHandler : IRequestHandler<RegisterUser, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;

    public RegisterUserHandler(IAuthorizationService authorizationService, IUserRepository userRepository, IPasswordManager passwordManager)
    {
		_authorizationService = authorizationService;
		_userRepository = userRepository;
		_passwordManager = passwordManager;
        _passwordManager = passwordManager;
    }

    public async Task<Unit> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var email = request.Email;
        var userName = request.UserName;
        var password = request.Password;

        await _userRepository.HasDataCurrentlyUsedAsync(email, userName);

        var hashedPassword = _passwordManager.Secure(password);
        var user = new User(email, hashedPassword, userName);
		
		await _userRepository.AddAsync(user);
        await _authorizationService.AddUserToRoleAsync(user.Id, "User");

		return Unit.Value;
    }
}
