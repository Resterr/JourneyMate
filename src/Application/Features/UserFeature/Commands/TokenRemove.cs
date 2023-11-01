using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Commands;

public record TokenRemove(Guid UserId) : IRequest<Unit>;

internal sealed class TokenRemoveHandler : IRequestHandler<TokenRemove, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public TokenRemoveHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(TokenRemove request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Include(x => x.Roles)
				.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken) ??
			throw new UserNotFoundException(request.UserId);

		user.RemoveRefreshToken();

		_dbContext.Users.Update(user);
		await _dbContext.SaveChangesAsync(cancellationToken);
		
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