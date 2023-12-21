using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class AdministrativeAreaConfiguration : IEntityTypeConfiguration<AdministrativeArea>
{
	public void Configure(EntityTypeBuilder<AdministrativeArea> builder)
	{
		builder.OwnsOne(x => x.Level1)
			.Property(x => x.ShortName)
			.HasColumnName("Level1ShortName")
			.HasMaxLength(256)
			.IsRequired();

		builder.OwnsOne(x => x.Level1)
			.Property(x => x.LongName)
			.HasColumnName("Level1LongName")
			.HasMaxLength(256)
			.IsRequired();

		builder.OwnsOne(x => x.Level2)
			.Property(x => x.ShortName)
			.HasColumnName("Level2ShortName")
			.HasMaxLength(256)
			.IsRequired();

		builder.OwnsOne(x => x.Level2)
			.Property(x => x.LongName)
			.HasColumnName("Level2LongName")
			.HasMaxLength(256)
			.IsRequired();

		builder.ToTable("AdministrativeArea");
	}
}