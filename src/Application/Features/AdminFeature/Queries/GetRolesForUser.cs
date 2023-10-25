using AutoMapper;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Queries;

public record GetRolesForUser(Guid Id) : IRequest<List<RoleDto>>;

internal sealed class GetRolesForHandler : IRequestHandler<GetRolesForUser, List<RoleDto>>
{
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;

	public GetRolesForHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<List<RoleDto>> Handle(GetRolesForUser request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		var result = _mapper.Map<List<RoleDto>>(user.Roles);

		return result;
	}
}