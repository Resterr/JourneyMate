﻿using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
	public void Configure(EntityTypeBuilder<Schedule> builder)
	{
		builder.Property(x => x.StartingDate)
			.IsRequired();

		builder.ToTable("Schedule");
	}
}