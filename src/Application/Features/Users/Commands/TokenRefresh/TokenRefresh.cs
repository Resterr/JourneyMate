using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Repositories;
using MediatR;
using System.Security.Claims;

namespace JourneyMate.Application.Features.Users.Commands.TokenRefresh;
[Authorize(Role = "User")]
public record TokenRefresh(string AccessToken, string RefreshToken) : IRequest<TokensDto>;

internal sealed class TokenRefreshHandler : IRequestHandler<TokenRefresh, TokensDto>
{
    private readonly IUserRepository _userRepository;
	private readonly ITokenService _tokenService;
	private readonly IDateTimeService _dateTimeService;

	public TokenRefreshHandler(IUserRepository userRepository, ITokenService tokenService, IDateTimeService dateTimeService)
    {
        _userRepository = userRepository;
		_tokenService = tokenService;
		_dateTimeService = dateTimeService;
    }

    public async Task<TokensDto> Handle(TokenRefresh request, CancellationToken cancellationToken)
    {
        string accessToken = request.AccessToken;
        string refreshToken = request.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

		var userId = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value ?? throw new BadRequestException("Invalid client request");
        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));

        if (!user.IsTokenValid(refreshToken, _dateTimeService.Now)) throw new BadRequestException("Invalid client request");

        var newAccessToken = _tokenService.GenerateAccessTokenFromClaims(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.SetRefreshToken(newRefreshToken);

        await _userRepository.UpdateAsync(user);

        return new TokensDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}
