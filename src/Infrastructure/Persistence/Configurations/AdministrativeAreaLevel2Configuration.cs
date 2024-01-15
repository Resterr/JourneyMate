using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class AdministrativeAreaLevel2Configuration : IEntityTypeConfiguration<AdministrativeAreaLevel2>
{
	public void Configure(EntityTypeBuilder<AdministrativeAreaLevel2> builder)
	{
		builder.Property(x => x.ShortName)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.LongName)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.AdministrativeAreaLevel1Id)
			.IsRequired();

		builder.ToTable("AdministrativeAreaLevel2");
	}
}