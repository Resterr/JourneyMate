namespace JourneyMate.Domain.Entities;

public class UserFollowerPlanRelation
{
	public Guid FollowerId { get; private set; }
	public UserFollower Follower { get; private set; }
	public Guid PlanId { get; private set; }
	public Plan Plan { get; private set; }

	private UserFollowerPlanRelation() { }
	public UserFollowerPlanRelation(UserFollower follower, Plan plan)
	{
		Follower = follower;
		Plan = plan;
	}
}