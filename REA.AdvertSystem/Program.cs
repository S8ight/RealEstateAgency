using FluentValidation;
using MediatR;
using MongoDB.Driver;
using System.Reflection;
using System.Text;
using DiscountGrpcService.Protos;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REA.AdvertSystem.Application.Adverts.Commands;
using REA.AdvertSystem.Application.Adverts.Queries;
using REA.AdvertSystem.Application.Common.Behaviours;
using REA.AdvertSystem.Application.Common.GrpcServices;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Consumers;
using REA.AdvertSystem.DataAccess;
using REA.AdvertSystem.DataAccess.Repositories;
using REA.AdvertSystem.Infrastructure.DataAccess;
using REA.AdvertSystem.Interfaces.Repositories;
using REA.AdvertSystem.Interfaces.Services;
using REA.AdvertSystem.Middlewares;
using REA.AdvertSystem.Services;
using Serilog;


var builder = WebApplication.CreateBuilder(args);


// builder.Services.Configure<MongoDatabaseSettings>(
//     builder.Configuration.GetSection("AdvertDatabase"));

builder.Services.AddDbContext<AdvertDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:DataBaseConnection"]!);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

// builder.Services.AddScoped<IAgencyDbConnection, AgencyDbConnection>();
builder.Services.AddTransient<IAdvertRepository, AdvertRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ISaveListRepository, SaveListRepository>();
builder.Services.AddTransient<IPhotoListRepository, PhotoListRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAdvertService, AdvertService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISaveListService, SaveListService>();
builder.Services.AddTransient<IPhotoListService, PhotoListService>();

/*builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserRegistrationConsumer>();
    x.AddConsumer<UserUpdateConsumer>();
    x.AddConsumer<UserDeleteConsumer>();
  
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["ConnectionStrings:RabbitMQConnection"]);
  
        cfg.ReceiveEndpoint("advert-user-queue", ep =>
        {
            ep.PrefetchCount = 20;
            ep.ConfigureConsumer<UserRegistrationConsumer>(context);
            ep.ConfigureConsumer<UserUpdateConsumer>(context);
            ep.ConfigureConsumer<UserDeleteConsumer>(context);
        });

    });
});*/

// builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
// (g =>
// {
//     g.Address = new Uri( builder.Configuration["ConnectionStrings:Grpc"]);
// })
//     .ConfigurePrimaryHttpMessageHandler(() =>
//     {
//         var handler = new HttpClientHandler();
//         handler.ServerCertificateCustomValidationCallback = 
//             HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
//
//         return handler;
//     });
//
// builder.Services.AddScoped<DiscountClientGrpc>();


// builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
// builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//
// builder.Services.AddMediatR(typeof(CreateAdvertCommand).GetTypeInfo().Assembly);
// builder.Services.AddMediatR(typeof(GetAdvertsPaginationList).GetTypeInfo().Assembly);
// builder.Services.AddMediatR(typeof(GetAdvertsById).GetTypeInfo().Assembly); 
// builder.Services.AddMediatR(typeof(DeleteAdvertCommand).GetTypeInfo().Assembly);
// builder.Services.AddMediatR(typeof(UpdateAdvertCommand).GetTypeInfo().Assembly);
//
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration["ConnectionStrings:Redis"];
//     options.InstanceName = "RedisAdvertsProject";
// });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:AccessTokenKey"] ?? string.Empty))
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



string myAllowedOrigins = "_myAllowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowedOrigins,
                          corsPolicyBuilder =>
                          {
                              corsPolicyBuilder.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

var app = builder.Build();

app.UseCors(myAllowedOrigins);

app.UseSerilogRequestLogging();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
