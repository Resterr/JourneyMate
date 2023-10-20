using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record RemoveAdminRole(Guid Id) : IRequest<Unit>;

internal sealed class RemoveAdminRoleHandler : IRequestHandler<RemoveAdminRole, Unit>
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IUserRepository _userRepository;

	public RemoveAdminRoleHandler(IUserRepository userRepository, IAuthorizationService authorizationService)
	{
		_userRepository = userRepository;
		_authorizationService = authorizationService;
	}

	public async Task<Unit> Handle(RemoveAdminRole request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);

		await _authorizationService.RemoveUserFromRoleAsync(user.Id, "Admin");

		return Unit.Value;
	}
}