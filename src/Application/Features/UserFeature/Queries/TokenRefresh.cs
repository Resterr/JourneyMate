using System.Security.Claims;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record TokenRefresh(string AccessToken, string RefreshToken) : IRequest<TokensDto>;

internal sealed class TokenRefreshHandler : IRequestHandler<TokenRefresh, TokensDto>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IDateTimeService _dateTimeService;
	private readonly IApplicationDbContext _dbContext;
	private readonly ITokenService _tokenService;

	public TokenRefreshHandler(IApplicationDbContext dbContext, ITokenService tokenService, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
	{
		_dbContext = dbContext;
		_tokenService = tokenService;
		_currentUserService = currentUserService;
		_dateTimeService = dateTimeService;
	}

	public async Task<TokensDto> Handle(TokenRefresh request, CancellationToken cancellationToken)
	{
		var accessToken = request.AccessToken;
		var refreshToken = request.RefreshToken;

		var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

		if (Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)) { }
		else
			throw new InvalidTokenException();

		var user = await _dbContext.Users.Include(x => x.Roles)
				.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken) ??
			throw new UserNotFoundException(userId);

		if (!user.IsTokenValid(refreshToken, _dateTimeService.CurrentDate())) throw new InvalidTokenException();

		var newAccessToken = _tokenService.GenerateAccessTokenFromClaims(principal.Claims);
		var newRefreshToken = _tokenService.GenerateRefreshToken();

		user.SetRefreshToken(newRefreshToken);

		_dbContext.Users.Update(user);
		await _dbContext.SaveChangesAsync(cancellationToken);

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