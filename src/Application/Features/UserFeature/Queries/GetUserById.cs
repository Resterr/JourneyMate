using MediatR;
using AutoMapper;
using JourneyMate.Domain.Repositories;
using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Common.Security;

namespace JourneyMate.Application.Features.UserFeature.Queries;
[Authorize(Role = "Admin")]
public record GetUserById(Guid Id) : IRequest<UserDto>;

internal sealed class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

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
