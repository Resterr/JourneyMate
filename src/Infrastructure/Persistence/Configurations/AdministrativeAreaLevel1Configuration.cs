using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class AdministrativeAreaLevel1Configuration : IEntityTypeConfiguration<AdministrativeAreaLevel1>
{
	public void Configure(EntityTypeBuilder<AdministrativeAreaLevel1> builder)
	{
		builder.Property(x => x.ShortName)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.LongName)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.CountryId)
			.IsRequired();

		builder.ToTable("AdministrativeAreaLevel1");
	}
}