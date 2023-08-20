using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Email)
            .HasMaxLength(500)
            .IsRequired();
		builder.Property(x => x.UserName)
			.HasMaxLength(100)
			.IsRequired();

        builder.ToTable("Users");
	}
}
