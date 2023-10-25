using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record GrantRole(Guid Id, string RoleName) : IRequest<Unit>;

internal sealed class GrantRoleHandler : IRequestHandler<GrantRole, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserRepository _userRepository;

	public GrantRoleHandler(IUserRepository userRepository, IAuthorizationService authorizationService)
	{
		_userRepository = userRepository;
		_authorizationService = authorizationService;
	}

	public async Task<Unit> Handle(GrantRole request, CancellationToken cancellationToken)
	{
		if (request.RoleName.ToLower() == "superadmin") throw new AccessForbiddenException();

		var user = await _userRepository.GetByIdAsync(request.Id);

		await _authorizationService.AddUserToRoleAsync(user.Id, request.RoleName);

		return Unit.Value;
	}
}

public class GrantRoleValidator : AbstractValidator<GrantRole>
{
	public GrantRoleValidator()
	{
		RuleFor(x => x.Id)
			.NotNull();
		RuleFor(x => x.RoleName)
			.NotNull();
	}
}