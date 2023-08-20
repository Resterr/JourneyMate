using JourneyMate.API.Middlewares;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;
using JourneyMate.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHealthChecks("/health");
app.UseHttpsRedirection();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();