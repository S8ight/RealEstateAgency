using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors();

var app = builder.Build();

app.UseAuthentication();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .WithOrigins("http://localhost:3000")
    .WithOrigins("http://localhost:3001")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

await app.UseOcelot();

app.Run();