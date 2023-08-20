using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using JourneyMate.Infrastructure.Services;
using JourneyMate.Infrastructure.Security;

namespace Microsoft.Extensions.DependencyInjection;
public static class Extensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddPersistence(configuration);
		services.AddSecurity(configuration);

		services.AddTransient<IDateTimeService, DateTimeService>();

		return services;
	}
}
