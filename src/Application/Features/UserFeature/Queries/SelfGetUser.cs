using AutoMapper;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record SelfGetUser : IRequest<UserDto>;

internal sealed class SelfGetUserHandler : IRequestHandler<SelfGetUser, UserDto>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;

	public SelfGetUserHandler(IUserRepository userRepository, IMapper mapper, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<UserDto> Handle(SelfGetUser request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _userRepository.GetByIdAsync(userId);
		var result = _mapper.Map<UserDto>(user);

		return result;
	}
}