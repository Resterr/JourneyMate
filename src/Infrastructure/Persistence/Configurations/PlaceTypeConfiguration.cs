using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PlaceTypeConfiguration : IEntityTypeConfiguration<PlaceType>
{
	public void Configure(EntityTypeBuilder<PlaceType> builder)
	{
		builder.Property(x => x.ApiName)
			.HasMaxLength(256)
			.IsRequired();

		builder.HasIndex(x => x.ApiName)
			.IsUnique();

		builder.Property(x => x.Name)
			.HasMaxLength(256)
			.IsRequired();

		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.ToTable("PlaceType");
	}
}