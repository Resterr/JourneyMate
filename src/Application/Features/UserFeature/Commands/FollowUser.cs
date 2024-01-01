using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Commands;

public record FollowUser(string UserName) : IRequest<Unit>;

internal sealed class FollowUserHandler : IRequestHandler<FollowUser, Unit>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IDateTimeService _dateTimeService;

	public FollowUserHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_dateTimeService = dateTimeService;
	}

	public async Task<Unit> Handle(FollowUser request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		
		if (user.UserName == request.UserName) throw new CannotFollowYourselfException();
		
		var follow = await _dbContext.Followers.Include(x => x.Followed)
			.Where(x => x.FollowerId == user.Id)
			.SingleOrDefaultAsync(x => x.Followed.UserName == request.UserName);
		
		if (follow != null) throw new AlreadyFollowedException(request.UserName);

		var userToFollow = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName) ?? throw new UserNotFoundException();
		follow = new Follow(user, userToFollow, _dateTimeService.CurrentDate());

		await _dbContext.Followers.AddAsync(follow);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}

public class FollowUserValidator : AbstractValidator<FollowUser>
{
	public FollowUserValidator()
	{
		RuleFor(x => x.UserName)
			.NotEmpty();
	}
}