using DiscountGrpcService.Repositories;
using DiscountGrpcService.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options => { 
    options.Configuration = builder.Configuration.GetValue<string>("ConnectionStrings:Redis");
    options.InstanceName = "AdvertCoupons";
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountGrpcService.Services.DiscountGrpcService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();