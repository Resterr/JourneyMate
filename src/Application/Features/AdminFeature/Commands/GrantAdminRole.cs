using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record GrantAdminRole(Guid Id) : IRequest<Unit>;

internal sealed class GrantAdminRoleHandler : IRequestHandler<GrantAdminRole, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserRepository _userRepository;

	public GrantAdminRoleHandler(IUserRepository userRepository, IAuthorizationService authorizationService)
	{
		_userRepository = userRepository;
		_authorizationService = authorizationService;
	}

	public async Task<Unit> Handle(GrantAdminRole request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);

		await _authorizationService.AddUserToRoleAsync(user.Id, "Admin");

		return Unit.Value;
	}
}