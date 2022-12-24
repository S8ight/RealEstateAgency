using FluentValidation;
using MediatR;
using MongoDB.Driver;
using System.Reflection;
using DiscountGrpcService.Protos;
using MassTransit;
using REA.AdvertSystem.Application.Adverts.Commands;
using REA.AdvertSystem.Application.Adverts.Queries;
using REA.AdvertSystem.Application.Common.Behaviours;
using REA.AdvertSystem.Application.Common.GrpcServices;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Application.Common.Mapping;
using REA.AdvertSystem.Application.Common.Models;
using REA.AdvertSystem.Infrastructure.DataAccess;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("AdvertDatabase:ConnectionString")));

builder.Services.AddScoped<IAgencyDbConnection, AgencyDbConnection>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
(g =>
{
    g.Address = new Uri( /*builder.Configuration["ConnectionStrings:Grpc"]*/"http://localhost:7180");
});
builder.Services.AddScoped<DiscountServiceGrpc>();


builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly);
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.AddMediatR(typeof(CreateAdvertCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetAdvertsPaginationList).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(GetAdvertsById).GetTypeInfo().Assembly); 
builder.Services.AddMediatR(typeof(DeleteAdvertCommand).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(UpdateAdvertCommand).GetTypeInfo().Assembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "RedisAdvertsProject";
});

// builder.Services.AddMassTransit(x =>
// {
//     x.AddConsumer<UserConsumer>();
//  
//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host("rabbitmq://localhost:5672");
//  
//         cfg.ReceiveEndpoint("advert-user-queue", ep =>
//         {
//             ep.PrefetchCount = 20;
//             ep.ConfigureConsumer<UserConsumer>(context);
//         });
//     });
// });

//builder.Services.AddHostedService<User>();


// builder.Services.AddMassTransit(x =>
// {
//     x.AddConsumer<UserConsumer>();
//     x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
//     {
//         cfg.Host(new Uri("rabbitmq://localhost:1334"),h =>
//         {
//             h.Username("guest");
//             h.Password("guest");
//         });
//         cfg.ReceiveEndpoint("userQueue", ep =>
//         {
//             ep.PrefetchCount = 16;
//             ep.UseMessageRetry(r => r.Interval(2, 100));
//             ep.ConfigureConsumer<UserConsumer>(provider);
//         });
//     }));
// });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var MyAllowedOrigins = "_myAllowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowedOrigins,
                          builder =>
                          {
                              builder.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

var app = builder.Build();

app.UseCors(MyAllowedOrigins);

app.UseHttpsRedirection();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
