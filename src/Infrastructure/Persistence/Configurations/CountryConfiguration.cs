using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
	public void Configure(EntityTypeBuilder<Country> builder)
	{
		builder.Property(x => x.ShortName)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.LongName)
			.HasMaxLength(256)
			.IsRequired();

		builder.ToTable("Country");
	}
}