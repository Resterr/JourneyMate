using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JourneyMate.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(x => x.Email)
			.HasMaxLength(512)
			.IsRequired();

		builder.Property(x => x.UserName)
			.HasMaxLength(256)
			.IsRequired();
		
		builder.HasMany(x => x.Reports)
			.WithOne(x => x.User)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.NoAction);
		
		builder.HasMany(x => x.Plans)
			.WithOne(x => x.User)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.NoAction);
		
		builder.HasMany(x => x.UserFollowers)
			.WithOne(x => x.Followed)
			.HasForeignKey(x => x.FollowedId)
			.OnDelete(DeleteBehavior.NoAction);
		
		builder.HasMany(x => x.UserFollowed)
			.WithOne(x => x.Follower)
			.HasForeignKey(x => x.FollowerId)
			.OnDelete(DeleteBehavior.NoAction);
		
		builder.ToTable("User");
	}
}