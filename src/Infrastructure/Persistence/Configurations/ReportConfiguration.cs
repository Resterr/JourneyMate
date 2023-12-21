using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
	public void Configure(EntityTypeBuilder<Report> builder)
	{
		builder.Property(x => x.UserId)
			.IsRequired();
		
		builder.Property(x => x.AddressId)
			.IsRequired();
		
		builder.Property(x => x.Rating)
			.IsRequired();
		
		builder.ToTable("Report");
	}
}