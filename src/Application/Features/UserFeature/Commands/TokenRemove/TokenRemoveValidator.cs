using FluentValidation;

namespace JourneyMate.Application.Features.UserFeature.Commands.TokenRemove;
public class TokenRemoveValidator : AbstractValidator<TokenRemove>
{
	public TokenRemoveValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();
	}
}
