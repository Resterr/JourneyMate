using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record LoginUser(string UserName, string Password) : IRequest<TokensDto>;

internal sealed class LoginUserHandler : IRequestHandler<LoginUser, TokensDto>
{
	private readonly IPasswordManager _passwordManager;
	private readonly ITokenService _tokenService;
	private readonly IUserRepository _userRepository;

	public LoginUserHandler(IUserRepository userRepository, IPasswordManager passwordManager, ITokenService tokenService)
	{
		_userRepository = userRepository;
		_passwordManager = passwordManager;
		_tokenService = tokenService;
	}

	public async Task<TokensDto> Handle(LoginUser request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByUserNameAsync(request.UserName);
		if (!_passwordManager.Validate(request.Password, user.PasswordHash)) throw new InvalidUserPassword();

		var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.UserName, user.Roles.Select(x => x.Name));
		var refreshToken = _tokenService.GenerateRefreshToken();
		var refreshTokenExpiryDate = _tokenService.GetRefreshExpiryDate();

		user.SetRefreshToken(refreshToken);
		user.SetRefreshTokenExpiryTime(refreshTokenExpiryDate);

		await _userRepository.UpdateAsync(user);

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