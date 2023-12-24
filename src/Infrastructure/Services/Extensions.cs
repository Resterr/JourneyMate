using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyMate.Infrastructure.Services;

internal static class Extensions
{
	private const string _urlsOptionsSectionName = "ApiUrls";
	private const string _keysOptionsSectionName = "ApiKeys";

	public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<ApiUrlsOptions>(configuration.GetRequiredSection(_urlsOptionsSectionName))
			.Configure<ApiKeysOptions>(configuration.GetRequiredSection(_keysOptionsSectionName));

		services.AddTransient<IDateTimeService, DateTimeService>();
		services.AddTransient<IGeocodeApiService, GeocodeApiService>();
		services.AddTransient<IPlacesApiService, PlacesApiService>();
		services.AddTransient<ICurrentUserService, CurrentUserService>();
		
		return services;
	}
}

public static class MathOperations
{
	private const double _earthRadius = 6371; // Radius of the Earth in kilometers

	public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
	{
		// Convert latitude and longitude from degrees to radians
		lat1 = DegreeToRadian(lat1);
		lon1 = DegreeToRadian(lon1);
		lat2 = DegreeToRadian(lat2);
		lon2 = DegreeToRadian(lon2);

		// Haversine formula
		var dLon = lon2 - lon1;
		var dLat = lat2 - lat1;
		var a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);
		var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		var distance = Math.Round(_earthRadius * c, 2);

		return distance;
	}

	private static double DegreeToRadian(double degree)
	{
		return degree * (Math.PI / 180);
	}

	public static Guid? ToGuid(this string? value)
	{
		return Guid.TryParse(value, out var result) ? result : null;
	}
}