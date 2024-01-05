using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
	public void Configure(EntityTypeBuilder<Address> builder)
	{
		builder.Property(x => x.ApiPlaceId)
			.HasMaxLength(512)
			.IsRequired();

		builder.HasIndex(x => x.ApiPlaceId)
			.IsUnique();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Latitude)
			.HasColumnName("Latitude")
			.IsRequired();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Longitude)
			.HasColumnName("Longitude")
			.IsRequired();

		builder.OwnsOne(x => x.Locality)
			.Property(x => x.ShortName)
			.HasColumnName("LocalityShortName")
			.HasMaxLength(256)
			.IsRequired();

		builder.OwnsOne(x => x.Locality)
			.Property(x => x.LongName)
			.HasColumnName("LocalityLongName")
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(x => x.AdministrativeAreaLevel2Id)
			.IsRequired();

		builder.Property(x => x.PostalCode)
			.HasMaxLength(16);

		builder.ToTable("Address");
	}
}