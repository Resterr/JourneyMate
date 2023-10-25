using AutoMapper;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Queries;

public record GetAllUsers : IRequest<List<UserDto>>;

internal sealed class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserDto>>
{
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;

	public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<List<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
	{
		var users = await _userRepository.GetAllAsync();
		var result = _mapper.Map<List<UserDto>>(users);

		return result;
	}
}