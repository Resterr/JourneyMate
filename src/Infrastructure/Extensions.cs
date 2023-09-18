using JourneyMate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using JourneyMate.Infrastructure.Services;
using JourneyMate.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyMate.Infrastructure;
public static class Extensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddPersistence(configuration);
		services.AddSecurity(configuration);
		services.AddServices(configuration);

		return services;
	}
}
