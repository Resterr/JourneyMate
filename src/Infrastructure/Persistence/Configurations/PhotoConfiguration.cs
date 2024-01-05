using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
	public void Configure(EntityTypeBuilder<Photo> builder)
	{
		builder.Property(x => x.PhotoReference)
			.IsRequired();

		builder.Property(x => x.Width)
			.IsRequired();

		builder.Property(x => x.Height)
			.IsRequired();

		builder.ToTable("Photo");
	}
}