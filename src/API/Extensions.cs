using System.ComponentModel;
using FluentValidation;
using JourneyMate.API.Converters;
using JourneyMate.API.Middlewares;
using JourneyMate.API.Policies;
using JourneyMate.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;

namespace JourneyMate.API;

public static class Extensions
{
	private const string _apiTitle = "JourneyMate API";
	private const string _apiVersion = "v1";

	public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
	{
		services.AddCorsPolicy();
		services.AddHttpContextAccessor();
		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>();
		services.AddScoped<ErrorHandlerMiddleware>();
		services.Configure<JsonOptions>(options =>
		{
			options.SerializerOptions.Converters.Add(new CustomGuidConverter());
		});
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
		app.UseCorsPolicy();
		app.UseHttpsRedirection();
		app.UseHealthChecks("/health");
		app.UseMiddleware<ErrorHandlerMiddleware>();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseSwagger();
		app.UseSwaggerUI(swagger =>
		{
			swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "JourneyMate");
		});
		app.UseReDoc(reDoc =>
		{
			reDoc.RoutePrefix = "docs";
			reDoc.SpecUrl($"/swagger/{_apiVersion}/swagger.json");
			reDoc.DocumentTitle = _apiTitle;
		});

		return app;
	}
}