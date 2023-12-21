using JourneyMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }
	DbSet<Role> Roles { get; }
	DbSet<Address> Addresses { get; }
	DbSet<Country> Countries { get; }
	DbSet<AdministrativeArea> AdministrativeArea { get; }
	DbSet<Place> Places { get; }
	DbSet<PlaceType> PlaceTypes { get; }
	DbSet<PlaceAddressRelation> PlaceAddress { get; }
	DbSet<Report> Reports { get; }
	DbSet<Plan> Plans { get; }
	DbSet<PlacePlanRelation> PlacePlans { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	int SaveChanges();
}