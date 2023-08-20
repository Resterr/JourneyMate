using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.Users.Commands.LoginUser;
[AllowAnonymous]
public record LoginUser(string UserName, string Password) : IRequest<TokensDto>;


internal sealed class LoginUserHandler : IRequestHandler<LoginUser, TokensDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
	private readonly ITokenService _tokenService;

    public LoginUserHandler(IUserRepository userRepository, IPasswordManager passwordManager, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
		_tokenService = tokenService;
    }

    public async Task<TokensDto> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserNameAsync(request.UserName);
        if (!_passwordManager.Validate(request.Password, user.PasswordHash)) throw new BadRequestException("Invalid password");

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.UserName);
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
