using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
	public void Configure(EntityTypeBuilder<Place> builder)
	{
		builder.Property(x => x.ApiPlaceId)
			.IsRequired();

		builder.HasIndex(x => x.ApiPlaceId)
			.IsUnique();

		builder.Property(x => x.AddressId)
			.IsRequired();

		builder.Property(x => x.BusinessStatus)
			.IsRequired();

		builder.Property(x => x.Name)
			.IsRequired();

		builder.Property(x => x.Rating)
			.IsRequired();

		builder.Property(x => x.UserRatingsTotal)
			.IsRequired();

		builder.Property(x => x.Vicinity)
			.IsRequired();

		builder.Property(x => x.DistanceFromAddress)
			.IsRequired();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Latitude)
			.IsRequired();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Longitude)
			.IsRequired();

		builder.OwnsOne(x => x.PlusCode)
			.Property(x => x.CompoundCode)
			.IsRequired();

		builder.OwnsOne(x => x.PlusCode)
			.Property(x => x.GlobalCode)
			.IsRequired();

		builder.OwnsOne(x => x.Photo)
			.Property(x => x.Height)
			.IsRequired();

		builder.OwnsOne(x => x.Photo)
			.Property(x => x.Width)
			.IsRequired();

		builder.OwnsOne(x => x.Photo)
			.Property(x => x.PhotoReference)
			.IsRequired();

		builder.ToTable("Place");
	}
}