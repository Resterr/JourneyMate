using System.Reflection;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Common;
using JourneyMate.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
	private readonly IMediator _mediator;

	public DbSet<User> Users => Set<User>();
	public DbSet<Role> Roles => Set<Role>();
	public DbSet<Address> Addresses => Set<Address>();
	public DbSet<Place> Places => Set<Place>();
	public DbSet<PlaceType> PlaceTypes => Set<PlaceType>();

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
	{
		_mediator = mediator;
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		builder.Entity<User>()
			.HasMany(x => x.Roles)
			.WithMany(x => x.Users)
			.UsingEntity(x => x.ToTable("UserRole"));

		builder.Entity<Place>()
			.HasMany(x => x.Types)
			.WithMany(x => x.Places)
			.UsingEntity(x => x.ToTable("PlaceTypeRelation"));

		builder.Entity<Place>()
			.HasOne(x => x.Address)
			.WithMany(x => x.Places)
			.HasForeignKey(x => x.AddressId)
			.OnDelete(DeleteBehavior.NoAction);

		base.OnModelCreating(builder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _mediator.DispatchDomainEvents(this);

		return await base.SaveChangesAsync(cancellationToken);
	}
}