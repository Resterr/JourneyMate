using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }
	DbSet<Role> Roles { get; }
	DbSet<Address> Addresses { get; }
	DbSet<Place> Places { get; }
	DbSet<PlaceType> PlaceTypes { get; }
	DbSet<PlaceAddress> PlaceAddress { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	int SaveChanges();
}