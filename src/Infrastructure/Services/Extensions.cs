using JourneyMate.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyMate.Infrastructure.Services;
internal static class Extensions
{	
	public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddTransient<IDateTimeService, DateTimeService>();

		return services;
    }
}
