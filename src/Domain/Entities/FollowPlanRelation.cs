namespace JourneyMate.Domain.Entities;

public class FollowPlanRelation
{
	public Guid FollowId { get; private set; }
	public Follow Follow { get; private set; }
	public Guid PlanId { get; private set; }
	public Plan Plan { get; private set; }

	private FollowPlanRelation() { }
	public FollowPlanRelation(Follow follow, Plan plan)
	{
		Follow = follow;
		Plan = plan;
	}
}