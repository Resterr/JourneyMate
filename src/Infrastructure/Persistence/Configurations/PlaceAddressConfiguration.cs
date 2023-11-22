using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PlaceAddressConfiguration : IEntityTypeConfiguration<PlaceAddress>
{
	public void Configure(EntityTypeBuilder<PlaceAddress> builder)
	{
		builder
			.HasKey(x => new { x.AddressId, x.PlaceId });

		builder
			.HasOne(pt => pt.Address)
			.WithMany(p => p.Places)
			.HasForeignKey(pt => pt.AddressId);

		builder
			.HasOne(pt => pt.Place)
			.WithMany(t => t.Addresses)
			.HasForeignKey(pt => pt.PlaceId);

		builder.ToTable("PlaceAddress");
	}
}