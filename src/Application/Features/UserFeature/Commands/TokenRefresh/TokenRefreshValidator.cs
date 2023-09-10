using FluentValidation;

namespace JourneyMate.Application.Features.UserFeature.Commands.TokenRefresh;
public class RefreshTokenValidator : AbstractValidator<TokenRefresh>
{
	public RefreshTokenValidator()
	{
		RuleFor(x => x.AccessToken).NotEmpty();
		RuleFor(x => x.RefreshToken).NotEmpty();
	}
}
