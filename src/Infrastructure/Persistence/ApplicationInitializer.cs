using JourneyMate.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JourneyMate.Infrastructure.Persistence;

public class ApplicationInitializer : IHostedService
{
	private readonly IHostEnvironment _env;
	private readonly IServiceProvider _serviceProvider;

	public ApplicationInitializer(IServiceProvider serviceProvider, IHostEnvironment env)
	{
		_serviceProvider = serviceProvider;
		_env = env;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using var scope = _serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


		if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
		{
			if (_env.IsDevelopment()) await dbContext.Database.MigrateAsync(cancellationToken);
		}
		else
		{
			var userSeeder = scope.ServiceProvider.GetRequiredService<IUsersSeeder>();
			if (await dbContext.Roles.AnyAsync(cancellationToken) == false)
			{
				var roles = userSeeder.SeedDefaultRoles();

				await dbContext.Roles.AddRangeAsync(roles, cancellationToken);
				await dbContext.SaveChangesAsync(cancellationToken);
			}

			if (await dbContext.Users.FirstOrDefaultAsync(x => x.Roles.Any(y => y.Name == "SuperAdmin"), cancellationToken) == null)
			{
				var superAdminRole = await dbContext.Roles.SingleAsync(x => x.Name == "SuperAdmin", cancellationToken);

				var superAdmin = userSeeder.SeedSuperAdmin();
				superAdmin.AddRole(superAdminRole);

				await dbContext.Users.AddAsync(superAdmin, cancellationToken);
				await dbContext.SaveChangesAsync(cancellationToken);
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}