using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
	public void Configure(EntityTypeBuilder<Follow> builder)
	{
		builder.Property(x => x.FollowDate)
			.IsRequired();
		
		builder.ToTable("Follow");
	}
}