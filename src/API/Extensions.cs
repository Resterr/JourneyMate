using JourneyMate.API.Middlewares;
using JourneyMate.API.Services;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;

namespace JourneyMate.API;
public static class Extensions
{
	public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
	{
		services.AddScoped<ErrorHandlingMiddleware>();
		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerDocument();

		return services;
	}

	public static WebApplication UsePresentationLayer(this WebApplication app)
	{
		app.UseHttpsRedirection();
		app.UseMiddleware<ErrorHandlingMiddleware>();
		app.UseHealthChecks("/health");	
		app.UseOpenApi();
		app.UseSwaggerUi3();
		app.MapControllers();
		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}
}

