using System.Security.Authentication;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Common.Options;
using JourneyMate.Infrastructure.Persistence.Interceptors;
using JourneyMate.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JourneyMate.Infrastructure.Persistence;

internal static class Extensions
{
	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		var options = configuration.GetOptions<ConnectionStringsOptions>("ConnectionStrings");
		
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();
		services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(options.SqlServer, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
		services.AddScoped(_ => {
			var settings = MongoClientSettings.FromUrl(new MongoUrl(options.MongoDb));
			settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
			return new ApplicationMongoClient(settings);
		});
		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
		services.AddScoped<IApplicationMongoClient>(provider => provider.GetRequiredService<ApplicationMongoClient>());
		services.AddScoped<ApplicationInitializer>();
		services.AddScoped<IUsersSeeder, UsersSeeder>();
		services.AddHostedService<ApplicationInitializer>();
		
		return services;
	}
}