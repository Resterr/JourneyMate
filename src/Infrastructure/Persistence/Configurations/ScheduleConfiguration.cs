using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
	public void Configure(EntityTypeBuilder<Schedule> builder)
	{
		builder.Property(x => x.StartingDate)
			.IsRequired();
		
		builder.HasOne(x => x.Place)
			.WithMany()
			.HasForeignKey(x => x.PlaceId)
			.IsRequired();

		builder.HasOne(x => x.Plan)
			.WithMany()
			.HasForeignKey(x => x.PlanId)
			.IsRequired();

		builder.ToTable("Schedule");
	}
}