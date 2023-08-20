using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.Users.Commands.TokenRemove;
[Authorize(Role = "User")]
public record TokenRemove(Guid UserId) : IRequest<Unit>;

internal sealed class TokenRemoveHandler : IRequestHandler<TokenRemove, Unit>
{
    private readonly IUserRepository _userRepository;

    public TokenRemoveHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(TokenRemove request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new NotFoundException("User not found");
        user.RemoveRefreshToken();

        await _userRepository.UpdateAsync(user);

        return Unit.Value;
    }
}
