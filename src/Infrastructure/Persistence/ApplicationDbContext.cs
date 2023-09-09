using System.Reflection;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Common;
using JourneyMate.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext
{
	private readonly IMediator _mediator;
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
	{
		_mediator = mediator;
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
	}

	public DbSet<User> Users => Set<User>();
	public DbSet<Role> Roles => Set<Role>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		builder.Entity<User>()
		   .HasMany(x => x.Roles)
		   .WithMany(x => x.Users)
		   .UsingEntity(j =>
				j.ToTable("UserRoles")
			);

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
