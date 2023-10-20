using JourneyMate.API;
using JourneyMate.API.Requests;
using JourneyMate.Application;
using JourneyMate.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
	.ReadFrom.Services(services)
	.Enrich.FromLogContext());

builder.Services.AddPresentationLayer();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

app.UsePresentationLayer();

app.RegisterUsersRequests();
app.RegisterAdminRequests();
app.RegisterAddressRequests();

app.Run();