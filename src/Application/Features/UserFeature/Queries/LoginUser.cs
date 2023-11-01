using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record LoginUser(string UserName, string Password) : IRequest<TokensDto>;

internal sealed class LoginUserHandler : IRequestHandler<LoginUser, TokensDto>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IPasswordManager _passwordManager;
	private readonly ITokenService _tokenService;

	public LoginUserHandler(IApplicationDbContext dbContext, IPasswordManager passwordManager, ITokenService tokenService)
	{
		_dbContext = dbContext;
		_passwordManager = passwordManager;
		_tokenService = tokenService;
	}

	public async Task<TokensDto> Handle(LoginUser request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Include(x => x.Roles)
				.SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken) ??
			throw new UserNotFoundException(request.UserName, "username");
		
		if (!_passwordManager.Validate(request.Password, user.PasswordHash)) throw new InvalidUserPassword();

		var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.UserName, user.Roles.Select(x => x.Name));
		var refreshToken = _tokenService.GenerateRefreshToken();
		var refreshTokenExpiryDate = _tokenService.GetRefreshExpiryDate();

		user.SetRefreshToken(refreshToken);
		user.SetRefreshTokenExpiryTime(refreshTokenExpiryDate);

		_dbContext.Users.Update(user);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return new TokensDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}
}

public class LoginUserValidator : AbstractValidator<LoginUser>
{
	public LoginUserValidator()
	{
		RuleFor(x => x.UserName)
			.NotNull();
		RuleFor(x => x.Password)
			.NotNull();
	}
}