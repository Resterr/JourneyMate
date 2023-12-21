using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
	public void Configure(EntityTypeBuilder<Place> builder)
	{
		builder.Property(x => x.ApiPlaceId)
			.HasMaxLength(512)
			.IsRequired();

		builder.HasIndex(x => x.ApiPlaceId)
			.IsUnique();

		builder.Property(x => x.BusinessStatus)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.Name)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.Rating)
			.IsRequired();

		builder.Property(x => x.UserRatingsTotal)
			.IsRequired();

		builder.Property(x => x.Vicinity)
			.HasMaxLength(512)
			.IsRequired();
		
		builder.OwnsOne(x => x.Location)
			.Property(x => x.Latitude)
			.HasColumnName("Latitude")
			.IsRequired();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Longitude)
			.HasColumnName("Longitude")
			.IsRequired();

		builder.OwnsOne(x => x.PlusCode)
			.Property(x => x.CompoundCode)
			.HasColumnName("CompoundCode")
			.HasMaxLength(256)
			.IsRequired();

		builder.OwnsOne(x => x.PlusCode)
			.Property(x => x.GlobalCode)
			.HasColumnName("GlobalCode")
			.HasMaxLength(256)
			.IsRequired();
		
		builder.ToTable("Place");
	}
}