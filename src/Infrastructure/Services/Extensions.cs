using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyMate.Infrastructure.Services;
internal static class Extensions
{
	private const string _urlsOptionsSectionName = "ApiUrls";
	private const string _keysOptionsSectionName = "ApiKeys";

	public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
		services
			.Configure<ApiUrlsOptions>(configuration.GetRequiredSection(_urlsOptionsSectionName))
			.Configure<ApiKeysOptions>(configuration.GetRequiredSection(_keysOptionsSectionName));

		services.AddTransient<IDateTimeService, DateTimeService>();
		services.AddTransient<IAvailabilityService, AvailabilityService>();
		services.AddTransient<IGeocodeApiService, GeocodeApiService>();

		return services;
    }
}
