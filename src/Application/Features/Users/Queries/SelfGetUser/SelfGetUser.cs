using AutoMapper;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.Users.Queries.SelfGetUser;
[Authorize(Role = "User")]
public record SelfGetUser() : IRequest<UserDto>;

internal sealed class SelfGetUserHandler : IRequestHandler<SelfGetUser, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public SelfGetUserHandler(IUserRepository userRepository, IMapper mapper, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    public async Task<UserDto> Handle(SelfGetUser request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
        var result = _mapper.Map<UserDto>(user);

        return result;
    }
}
