using FluentValidation;
using JourneyMate.API.Middlewares;
using JourneyMate.API.Services;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;

namespace JourneyMate.API;

public static class Extensions
{
	private const string _apiTitle = "JourneyMate API";
	private const string _apiVersion = "v1";

	public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>();
		services.AddScoped<ErrorHandlerMiddleware>();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.AddRouting(options => options.LowercaseUrls = true);
		services.AddEndpointsApiExplorer();
		ValidatorOptions.Global.LanguageManager.Enabled = false;
		services.AddSwaggerGen(swagger =>
		{
			swagger.EnableAnnotations();
			swagger.CustomSchemaIds(x => x.FullName);
			swagger.SwaggerDoc(_apiVersion, new OpenApiInfo
			{
				Title = _apiTitle,
				Version = _apiVersion
			});
		});

		return services;
	}

	public static WebApplication UsePresentationLayer(this WebApplication app)
	{
		app.UseHttpsRedirection();
		app.UseHealthChecks("/health");
		app.UseMiddleware<ErrorHandlerMiddleware>();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseSwagger();
		app.UseSwaggerUI(swagger => { swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "JourneyMate"); });
		app.UseReDoc(reDoc =>
		{
			reDoc.RoutePrefix = "docs";
			reDoc.SpecUrl($"/swagger/{_apiVersion}/swagger.json");
			reDoc.DocumentTitle = _apiTitle;
		});

		return app;
	}
}