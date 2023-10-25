using FluentValidation;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.UserFeature.Commands;

public record TokenRemove(Guid UserId) : IRequest<Unit>;

internal sealed class TokenRemoveHandler : IRequestHandler<TokenRemove, Unit>
{
	private readonly IUserRepository _userRepository;

	public TokenRemoveHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Unit> Handle(TokenRemove request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.UserId);
		user.RemoveRefreshToken();

		await _userRepository.UpdateAsync(user);

		return Unit.Value;
	}
}

public class TokenRemoveValidator : AbstractValidator<TokenRemove>
{
	public TokenRemoveValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty();
	}
}