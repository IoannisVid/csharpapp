using System.Reflection;
using CSharpApp.Api.Middlewares;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddMediatRConfiguration();
builder.Services.AddFluentValidationConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseMiddleware<RequestPerformanceMiddleware>();
}

//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();
versionedEndpointRouteBuilder.MapProductRoutes();
versionedEndpointRouteBuilder.MapCategoryRoutes();

app.Run();