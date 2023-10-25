using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record TokenRefresh(string AccessToken, string RefreshToken) : IRequest<TokensDto>;

internal sealed class TokenRefreshHandler : IRequestHandler<TokenRefresh, TokensDto>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IDateTimeService _dateTimeService;
	private readonly ITokenService _tokenService;
	private readonly IUserRepository _userRepository;

	public TokenRefreshHandler(IUserRepository userRepository, ITokenService tokenService, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
	{
		_userRepository = userRepository;
		_tokenService = tokenService;
		_currentUserService = currentUserService;
		_dateTimeService = dateTimeService;
	}

	public async Task<TokensDto> Handle(TokenRefresh request, CancellationToken cancellationToken)
	{
		var accessToken = request.AccessToken;
		var refreshToken = request.RefreshToken;

		var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

		var userId = _currentUserService.UserId;
		var user = await _userRepository.GetByIdAsync((Guid)userId!);

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
		RuleFor(x => x.AccessToken)
			.NotNull();
		RuleFor(x => x.RefreshToken)
			.NotNull();
	}
}