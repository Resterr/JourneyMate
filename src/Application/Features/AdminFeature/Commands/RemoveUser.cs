using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record RemoveUser(Guid Id) : IRequest<Unit>;

internal sealed class RemoveUserHandler : IRequestHandler<RemoveUser, Unit>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public RemoveUserHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Unit> Handle(RemoveUser request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Include(x => x.Roles)
				.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
			throw new UserNotFoundException(request.Id);
		var roles = user.Roles.Select(x => x.Name.ToLower())
			.ToList();

		if (roles.Contains("superadmin")) throw new AccessForbiddenException();
		if (roles.Contains("admin"))
		{
			var userId = _currentUserService.UserId ?? throw new AccessForbiddenException();
			var currentUser = await _dbContext.Users.Include(x => x.Roles)
				.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

			if (currentUser == null) throw new AccessForbiddenException();

			if (!currentUser.Roles.Select(x => x.Name)
				.Contains("SuperAdmin"))
				throw new AccessForbiddenException();
		}

		_dbContext.Users.Remove(user);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}

public class RemoveUserValidator : AbstractValidator<RemoveUser>
{
	public RemoveUserValidator()
	{
		RuleFor(x => x.Id)
			.NotNull();
	}
}