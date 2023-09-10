using FluentValidation;

namespace JourneyMate.Application.Features.UserFeature.Queries.GetUserById;
public class GetUserByIdValidator : AbstractValidator<GetUserById>
{
	public GetUserByIdValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
	}
}
