using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
	public void Configure(EntityTypeBuilder<Address> builder)
	{
		builder.Property(x => x.ApiPlaceId)
			.IsRequired();

		builder.HasIndex(x => x.ApiPlaceId)
			.IsUnique();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Latitude)
			.IsRequired();

		builder.OwnsOne(x => x.Location)
			.Property(x => x.Longitude)
			.IsRequired();

		builder.Property(x => x.Locality)
			.IsRequired();
		
		builder.Property(x => x.AdministrativeAreaLevel2)
			.IsRequired();
		
		builder.Property(x => x.AdministrativeAreaLevel1)
			.IsRequired();
		
		builder.Property(x => x.Country)
			.IsRequired();
		
		builder.Property(x => x.PostalCode)
			.IsRequired();
		
		builder.ToTable("Address");
	}
}