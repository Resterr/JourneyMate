using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record RemoveUser(Guid Id) : IRequest<Unit>;

internal sealed class RemoveUserHandler : IRequestHandler<RemoveUser, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly ICurrentUserService _currentUserService;
	private readonly IUserRepository _userRepository;

	public RemoveUserHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IAuthorizationService authorizationService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
		_authorizationService = authorizationService;
	}

	public async Task<Unit> Handle(RemoveUser request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		var roles = user.Roles.Select(x => x.Name.ToLower())
			.ToList();

		if (roles.Contains("superadmin")) throw new AccessForbiddenException();
		if (roles.Contains("admin"))
		{
			var userId = _currentUserService.UserId ?? throw new AccessForbiddenException();
			await _authorizationService.AuthorizeAsync(userId, "superadmin");
		}

		await _userRepository.DeleteAsync(user);

		return Unit.Value;
	}
}

public class RemoveUserValidator : AbstractValidator<RemoveUser>
{
	public RemoveUserValidator()
	{
		RuleFor(x => x.Id)
			.NotNull();
	}
}