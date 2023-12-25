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
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var shareUser = await _dbContext.Users.Include(x => x.UserFollowers).SingleOrDefaultAsync(x => x.UserName == request.Username) ?? throw new UserNotFoundException();
		var plan = await _dbContext.Plans.SingleOrDefaultAsync(x => x.Id == request.PlanId) ?? throw new PlanNotFound(request.PlanId);

		var follow = await _dbContext.Followers.SingleOrDefaultAsync(x => x.FollowerId == shareUser.Id && x.FollowedId == user.Id);
		if (follow != null)
		{
			var planShare = new UserFollowerPlanRelation(follow, plan);
			plan.AddShare(planShare);

			_dbContext.Plans.Update(plan);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
		else
		{
			throw new InvalidFollowerException(request.Username);
		}
	}
}