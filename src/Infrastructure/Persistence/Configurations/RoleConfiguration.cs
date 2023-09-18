using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Infrastructure.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.Property(x => x.Name)
			.HasMaxLength(25)
			.IsRequired();

		builder.ToTable("Role");
	}
}