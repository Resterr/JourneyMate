using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }
	DbSet<Role> Roles { get; }
	DbSet<Address> Addresses { get; }
	DbSet<Place> Places { get; }
	DbSet<PlaceType> PlaceTypes { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	int SaveChanges();
}