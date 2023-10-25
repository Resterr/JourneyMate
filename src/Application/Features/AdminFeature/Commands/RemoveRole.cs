using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record RemoveRole(Guid Id, string RoleName) : IRequest<Unit>;

internal sealed class RemoveRoleHandler : IRequestHandler<RemoveRole, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserRepository _userRepository;

	public RemoveRoleHandler(IUserRepository userRepository, IAuthorizationService authorizationService)
	{
		_userRepository = userRepository;
		_authorizationService = authorizationService;
	}

	public async Task<Unit> Handle(RemoveRole request, CancellationToken cancellationToken)
	{
		if (request.RoleName.ToLower() == "superadmin") throw new AccessForbiddenException();

		var user = await _userRepository.GetByIdAsync(request.Id);
		var roles = user.Roles.Select(x => x.Name)
			.ToList();

		if (roles.Contains("SuperAdmin")) throw new AccessForbiddenException();

		await _authorizationService.RemoveUserFromRoleAsync(user.Id, request.RoleName);

		return Unit.Value;
	}
}

public class RemoveRoleValidator : AbstractValidator<GrantRole>
{
	public RemoveRoleValidator()
	{
		RuleFor(x => x.Id)
			.NotNull();
		RuleFor(x => x.RoleName)
			.NotNull();
	}
}