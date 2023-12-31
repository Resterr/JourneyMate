using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PlaceAddressRelationConfiguration : IEntityTypeConfiguration<PlaceAddressRelation>
{
	public void Configure(EntityTypeBuilder<PlaceAddressRelation> builder)
	{
		builder
			.HasKey(x => new { x.AddressId, x.PlaceId });

		builder
			.HasOne(x => x.Address)
			.WithMany(x => x.Places)
			.HasForeignKey(x => x.AddressId);

		builder
			.HasOne(x => x.Place)
			.WithMany(x => x.Addresses)
			.HasForeignKey(x => x.PlaceId);
		
		builder.Property(x => x.DistanceFromAddress)
			.HasPrecision(8, 2)
			.IsRequired();

		builder.ToTable("PlaceAddressRelation");
	}
}