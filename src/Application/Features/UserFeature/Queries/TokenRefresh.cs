using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;
using System.Security.Claims;

namespace JourneyMate.Application.Features.UserFeature.Queries;
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
        var accessToken = request.AccessToken;
        var refreshToken = request.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

        var userId = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier) ?? throw new UserNotFoundException();
        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId.Value));

        if (!user.IsTokenValid(refreshToken, _dateTimeService.CurrentDate())) throw new InvalidTokenException();

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

public class TokenRefreshValidator : AbstractValidator<TokenRefresh>
{
	public TokenRefreshValidator()
	{
		RuleFor(x => x.AccessToken).NotNull();
		RuleFor(x => x.RefreshToken).NotNull();
	}
}
