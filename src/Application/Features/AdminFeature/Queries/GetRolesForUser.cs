using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Security;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Queries;
[Authorize(Role = "Admin")]
public record GetRolesForUser(Guid Id) : IRequest<List<string>>;

internal sealed class GetUserByIdHandler : IRequestHandler<GetRolesForUser, List<string>>
{
	private readonly IAuthorizationService _authorizationService;

	public GetUserByIdHandler(IAuthorizationService authorizationService)
	{
		_authorizationService = authorizationService;
	}
	public async Task<List<string>> Handle(GetRolesForUser request, CancellationToken cancellationToken)
	{
		var result = await _authorizationService.GetRolesForUserAsync(request.Id);

		return result.ToList();
	}
}