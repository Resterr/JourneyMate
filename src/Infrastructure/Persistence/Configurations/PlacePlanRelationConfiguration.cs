﻿using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PlacePlanRelationConfiguration : IEntityTypeConfiguration<PlacePlanRelation>
{
	public void Configure(EntityTypeBuilder<PlacePlanRelation> builder)
	{
		builder
			.HasKey(x => new { x.PlaceId, x.PlanId });

		builder
			.HasOne(x => x.Place)
			.WithMany(x => x.Plans)
			.HasForeignKey(x => x.PlaceId);

		builder
			.HasOne(x => x.Plan)
			.WithMany(x => x.Places)
			.HasForeignKey(x => x.PlanId);
		
		builder.ToTable("PlacePlanRelation");
	}
}