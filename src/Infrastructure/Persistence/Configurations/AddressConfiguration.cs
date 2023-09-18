using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
	public void Configure(EntityTypeBuilder<Address> builder)
	{
		builder
			.Property(x => x.PlaceId)
			.IsRequired();

		builder
			.HasIndex(x => x.PlaceId)
			.IsUnique();

		builder
			.OwnsOne(x => x.Location)
			.Property(x => x.Latitude)
			.IsRequired();

		builder
			.OwnsOne(x => x.Location)
			.Property(x => x.Longitude)
			.IsRequired();

		builder
			.OwnsOne(x => x.Locality)
			.Property(x => x.ShortName)
			.IsRequired();

		builder
			.OwnsOne(x => x.Locality)
			.Property(x => x.LongName)
			.IsRequired();

		builder
			.OwnsOne(x => x.AdministrativeAreaLevel2)
			.Property(x => x.ShortName)
			.IsRequired();

		builder
			.OwnsOne(x => x.AdministrativeAreaLevel2)
			.Property(x => x.LongName)
			.IsRequired();

		builder
			.OwnsOne(x => x.AdministrativeAreaLevel1)
			.Property(x => x.ShortName)
			.IsRequired();

		builder
			.OwnsOne(x => x.AdministrativeAreaLevel1)
			.Property(x => x.LongName)
			.IsRequired();

		builder
			.OwnsOne(x => x.Country)
			.Property(x => x.ShortName)
			.IsRequired();

		builder
			.OwnsOne(x => x.Country)
			.Property(x => x.LongName)
			.IsRequired();

		builder
			.OwnsOne(x => x.PostalCode)
			.Property(x => x.ShortName)
			.IsRequired();

		builder
			.OwnsOne(x => x.PostalCode)
			.Property(x => x.LongName)
			.IsRequired();

		builder.ToTable("Address");
	}
}
