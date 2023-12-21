using System.Reflection;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Common;
using JourneyMate.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
	private readonly IMediator _mediator;

	public DbSet<User> Users => Set<User>();
	public DbSet<Role> Roles => Set<Role>();
	public DbSet<Address> Addresses => Set<Address>();
	public DbSet<Country> Countries => Set<Country>();
	public DbSet<AdministrativeArea> AdministrativeArea => Set<AdministrativeArea>();
	public DbSet<Place> Places => Set<Place>();
	public DbSet<PlaceType> PlaceTypes => Set<PlaceType>();
	public DbSet<PlaceAddressRelation> PlaceAddress => Set<PlaceAddressRelation>();
	public DbSet<Report> Reports => Set<Report>();
	public DbSet<Plan> Plans => Set<Plan>();
	public DbSet<PlacePlanRelation> PlacePlans => Set<PlacePlanRelation>();
	
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
			.UsingEntity(
				"UserRoleRelation",
				x => x.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
				x => x.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
				x => x.HasKey("UserId", "RoleId"));

		builder.Entity<Place>()
			.HasMany(x => x.Types)
			.WithMany(x => x.Places)
			.UsingEntity(
				"PlacePlaceTypeRelation",
				x => x.HasOne(typeof(PlaceType)).WithMany().HasForeignKey("PlaceTypeId").HasPrincipalKey(nameof(PlaceType.Id)),
				x => x.HasOne(typeof(Place)).WithMany().HasForeignKey("PlaceId").HasPrincipalKey(nameof(Place.Id)),
				x => x.HasKey("PlaceId", "PlaceTypeId"));
		
		builder.Entity<Report>()
			.HasMany(x => x.Types)
			.WithMany(x => x.Reports)
			.UsingEntity(
				"ReportPlaceTypeRelation",
				x => x.HasOne(typeof(PlaceType)).WithMany().HasForeignKey("PlaceTypeId").HasPrincipalKey(nameof(PlaceType.Id)),
				x => x.HasOne(typeof(Report)).WithMany().HasForeignKey("ReportId").HasPrincipalKey(nameof(Report.Id)),
				x => x.HasKey("ReportId", "PlaceTypeId"));
		
		builder.Entity<Report>()
			.HasMany(x => x.Places)
			.WithMany(x => x.Reports)
			.UsingEntity(
				"ReportPlaceRelation",
				x => x.HasOne(typeof(Place)).WithMany().HasForeignKey("PlaceId").HasPrincipalKey(nameof(Place.Id)),
				x => x.HasOne(typeof(Report)).WithMany().HasForeignKey("ReportId").HasPrincipalKey(nameof(Report.Id)),
				x => x.HasKey("ReportId", "PlaceId"));
		
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