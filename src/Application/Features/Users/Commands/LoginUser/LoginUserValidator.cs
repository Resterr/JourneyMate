using FluentValidation;

namespace JourneyMate.Application.Features.Users.Commands.LoginUser;
public class LoginUserValidator : AbstractValidator<LoginUser>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
