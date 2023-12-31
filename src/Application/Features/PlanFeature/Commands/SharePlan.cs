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
		var user = await _dbContext.Users.Include(x => x.UserFollowers)
				.ThenInclude(x => x.Shared)
				.SingleOrDefaultAsync(x => x.Id == userId) ??
			throw new UserNotFoundException("name", request.Username);
		var shareUser = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == request.Username) ?? throw new UserNotFoundException("name", request.Username);
		var plan = await _dbContext.Plans.Include(x => x.Shared).ThenInclude(x => x.Follow).SingleOrDefaultAsync(x => x.Id == request.PlanId) ?? throw new PlanNotFoundException(request.PlanId);
		var follow = await _dbContext.Followers.Include(x => x.Shared).SingleOrDefaultAsync(x => x.FollowerId == shareUser.Id) ?? throw new InvalidFollowerException(request.Username);
		if (!follow.Shared.Any(x => x.FollowId == follow.Id))
		{
			var planShare = new FollowPlanRelation(follow, plan);
		
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