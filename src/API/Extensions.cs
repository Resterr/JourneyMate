using JourneyMate.API.Middlewares;
using JourneyMate.API.Services;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;

namespace JourneyMate.API;
public static class Extensions
{
	public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
		services.AddScoped<ErrorHandlingMiddleware>();	
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.AddRouting(options => options.LowercaseUrls = true);
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerDocument();

		return services;
	}

	public static WebApplication UsePresentationLayer(this WebApplication app)
	{
		app.UseHttpsRedirection();
		app.UseHealthChecks("/health");
		app.UseMiddleware<ErrorHandlingMiddleware>();
		app.MapControllers();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseOpenApi();
		app.UseSwaggerUi3();

		return app;
	}
}

