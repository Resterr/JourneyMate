using AutoMapper;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AdminFeature.Queries;

public record GetUserById(Guid Id) : IRequest<UserDto>;

internal sealed class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
{
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;

	public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<UserDto> Handle(GetUserById request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		var result = _mapper.Map<UserDto>(user);

		return result;
	}
}