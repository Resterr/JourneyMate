using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class UserFollower : BaseEntity
{
	public Guid FollowedId { get; private set; }
	public User Followed { get; private set; }
	public Guid FollowerId { get; private set; }
	public User Follower { get; private set; }
	public DateTime FollowDate { get; private set; }
	public List<UserFollowerPlanRelation> Shared { get; private set; } = new();

	private UserFollower() { }
	public UserFollower(User follower, User followed, DateTime followDate)
	{
		Follower = follower;
		Followed = followed;
		FollowDate = followDate;
	}
}