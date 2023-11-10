namespace JourneyMate.API.Policies;

internal static class CorsPolicy
{
	public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
	{
		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
			.Build();

		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder =>
			{
				builder.WithOrigins(config["Cors:AllowedOrigins"]!
						.Split(","))
					.WithMethods(config["Cors:AllowedMethods"]!
						.Split(","))
					.WithHeaders(config["Cors:AllowedHeaders"]!
						.Split(","));
			});
		});

		return services;
	}

	public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
	{
		app.UseCors("CorsPolicy");

		return app;
	}
}