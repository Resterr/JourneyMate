using FluentValidation;

namespace JourneyMate.Application.Features.UserFeature.Commands.RegisterUser;
public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
	public RegisterUserValidator()
	{
		RuleFor(x => x.Email).NotEmpty().MaximumLength(128).EmailAddress();
		RuleFor(x => x.UserName).NotEmpty().MaximumLength(128);
		RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(128);
		RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(6).MaximumLength(128).Equal(x => x.Password);
	}
}
