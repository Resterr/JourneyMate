namespace JourneyMate.Domain.Entities;

public class FollowPlanRelation
{
	public Guid FollowId { get; }
	public Follow Follow { get; private set; }
	public Guid PlanId { get; }
	public Plan Plan { get; private set; }

	private FollowPlanRelation() { }

	public FollowPlanRelation(Follow follow, Plan plan)
	{
		Follow = follow;
		Plan = plan;
	}
}