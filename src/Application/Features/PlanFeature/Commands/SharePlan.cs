using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlanFeature.Commands;

public record SharePlan(Guid PlanId, string Username) : IRequest<Unit>;

internal sealed class SharePlanHandler : IRequestHandler<SharePlan, Unit>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _dbContext;

	public SharePlanHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Unit> Handle(SharePlan request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.Include(x => x.UserFollowers).ThenInclude(x => x.Follower)
				.SingleOrDefaultAsync(x => x.Id == userId) ??
			throw new UserNotFoundException();
		
		if (user.UserName == request.Username) throw new CannotShareToYourselfException();
		var follow = user.UserFollowers.SingleOrDefault(x => x.Follower.UserName == request.Username && x.FollowedId == user.Id);
		if (follow is null) throw new InvalidFollowerException(request.Username);
		
		var plan =  await _dbContext.Plans.Include(x => x.Shared).ThenInclude(x => x.Follow).SingleOrDefaultAsync(x => x.Id == request.PlanId) ?? throw new PlanNotFoundException(request.PlanId);
		var planShare = plan.Shared.SingleOrDefault(x => x.Follow.Follower.UserName == request.Username && x.Follow.Followed.UserName == user.UserName);

		if (planShare == null)
		{
			planShare = new FollowPlanRelation(follow, plan);

			plan.AddShare(planShare);
			_dbContext.Plans.Update(plan);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
		else
		{
			throw new AlreadySharedException(request.PlanId, request.Username);
		}

		return Unit.Value;
	}
}