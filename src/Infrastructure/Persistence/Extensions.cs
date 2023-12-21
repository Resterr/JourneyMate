using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Common.Options;
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
		var options = configuration.GetOptions<ConnectionStringsOptions>("ConnectionStrings");
		
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();
		services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(options.SqlServer, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
		services.AddScoped<ApplicationInitializer>();
		services.AddScoped<IUsersSeeder, UsersSeeder>();
		services.AddScoped<ITypesSeeder, TypesSeeder>();
		services.AddHostedService<ApplicationInitializer>();
		
		return services;
	}
}