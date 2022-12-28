using AgencyAggregator.Interfaces;
using AgencyAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IUserService, UserService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ConnectionStrings:UserApi"]));

builder.Services.AddHttpClient<IAdvertService, AdvertService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ConnectionStrings:AdvertApi"]));

builder.Services.AddHttpClient<IDiscountService, DiscountAggService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ConnectionStrings:DiscountApi"]));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapControllers();

app.Run();