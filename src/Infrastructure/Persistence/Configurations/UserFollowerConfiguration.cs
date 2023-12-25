using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class UserFollowerConfiguration : IEntityTypeConfiguration<UserFollower>
{
	public void Configure(EntityTypeBuilder<UserFollower> builder)
	{
		builder.Property(x => x.FollowDate)
			.IsRequired();
		
		builder
			.HasOne(x => x.Follower)
			.WithMany(x => x.UserFollowers)
			.HasForeignKey(x => x.FollowerId)
			.OnDelete(DeleteBehavior.NoAction);

		builder
			.HasOne(x => x.Followed)
			.WithMany()
			.HasForeignKey(x => x.FollowedId)
			.OnDelete(DeleteBehavior.NoAction);
		
		builder.ToTable("UserFollower");
	}
}