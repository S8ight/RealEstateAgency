using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using REA.AuthorizationSystem.BLL.Authorization;
using REA.AuthorizationSystem.BLL.Authorization.Helpers;
using REA.AuthorizationSystem.BLL.DTO.Request;
using REA.AuthorizationSystem.BLL.Interfaces;
using REA.AuthorizationSystem.BLL.Services;
using REA.AuthorizationSystem.DAL.Context;
using REA.AuthorizationSystem.DAL.Data.Repositories;
using REA.AuthorizationSystem.DAL.Entities;
using REA.AuthorizationSystem.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AgencyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

// configure strongly typed settings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtConfiguration, JwtConfiguration>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.ImplicitlyValidateChildProperties = true;
    options.ImplicitlyValidateRootCollectionElements = true;
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<AgencyContext>()
    .AddDefaultTokenProviders();


builder.Services.AddMassTransit(x =>
{
    
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri("rabbitmq://rabbitmq:5672"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        config.Message<QueueRequest>(x=>{ });
    }));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();