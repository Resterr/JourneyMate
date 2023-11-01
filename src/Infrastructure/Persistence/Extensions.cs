using JourneyMate.Infrastructure.Persistence.Interceptors;
using JourneyMate.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyMate.Infrastructure.Persistence;

internal static class Extensions
{
	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();

		services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
		
		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
		services.AddScoped<ApplicationInitializer>();
		services.AddScoped<IUsersSeeder, UsersSeeder>();
		services.AddHostedService<ApplicationInitializer>();

		return services;
	}
}