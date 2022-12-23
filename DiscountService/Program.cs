using DiscountService.Repositories;
using DiscountService.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options => { 
    options.Configuration = builder.Configuration.GetValue<string>("ConnectionStrings:Redis");
    options.InstanceName = "AdvertCoupons";
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();