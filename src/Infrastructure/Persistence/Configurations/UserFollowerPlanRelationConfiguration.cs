using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class UserFollowerPlanRelationConfiguration : IEntityTypeConfiguration<UserFollowerPlanRelation>
{
	public void Configure(EntityTypeBuilder<UserFollowerPlanRelation> builder)
	{
		builder
			.HasKey(x => new { x.FollowerId, x.PlanId });

		builder
			.HasOne(x => x.Follower)
			.WithMany(x => x.Shared)
			.HasForeignKey(x => x.FollowerId);

		builder
			.HasOne(x => x.Plan)
			.WithMany(x => x.Shared)
			.HasForeignKey(x => x.PlanId);
		
		builder.ToTable("UserFollowerPlanRelation");
	}
} 