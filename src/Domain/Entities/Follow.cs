using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Follow : BaseEntity
{
	public Guid FollowedId { get; private set; }
	public User Followed { get; private set; }
	public Guid FollowerId { get; private set; }
	public User Follower { get; private set; }
	public DateTime FollowDate { get; private set; }
	public List<FollowPlanRelation> Shared { get; private set; } = new();
	
	private Follow() { }
	public Follow(User follower, User followed, DateTime followDate)
	{
		Follower = follower;
		Followed = followed;
		FollowDate = followDate;
	}
}