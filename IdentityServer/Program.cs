using IdentityServer.Configuration;
using IdentityServer.Context;
using IdentityServer.Entities;
using IdentityServer.Seeds;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContextIdentity>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DbContextIdentity>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<User>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt.MigrationsAssembly("IdentityServer"));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt.MigrationsAssembly("IdentityServer"));
    })
    .AddDeveloperSigningCredential()
    .AddProfileService<ProfileService<User>>();

SeedIdenityData.SeedData(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

app.UseIdentityServer();
app.UseRouting();

app.MapGet("/", () => "Hello World!");

app.Run();