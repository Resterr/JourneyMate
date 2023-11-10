using JourneyMate.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JourneyMate.Infrastructure.Persistence;

public class ApplicationInitializer : IHostedService
{
	private readonly IServiceProvider _serviceProvider;

	public ApplicationInitializer(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using var scope = _serviceProvider.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		if (context.Database.GetPendingMigrations()
			.Any())
		{
			await context.Database.MigrateAsync(cancellationToken);
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}