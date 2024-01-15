using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class FollowPlanRelationConfiguration : IEntityTypeConfiguration<FollowPlanRelation>
{
	public void Configure(EntityTypeBuilder<FollowPlanRelation> builder)
	{
		builder.HasKey(x => new { x.FollowId, x.PlanId });

		builder.HasOne(x => x.Follow)
			.WithMany(x => x.Shared)
			.HasForeignKey(x => x.FollowId);

		builder.HasOne(x => x.Plan)
			.WithMany(x => x.Shared)
			.HasForeignKey(x => x.PlanId);

		builder.ToTable("FollowPlanRelation");
	}
}