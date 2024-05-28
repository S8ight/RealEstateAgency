using System.Data;
using System.Reflection;
using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REA.ChatSystem.BLL.Consumers;
using REA.ChatSystem.BLL.Hubs;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.BLL.Services;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Repositories;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
ConfigureLogger();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped((s) => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AgencyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddScoped<IDbTransaction>(s =>
{
    SqlConnection conn = s.GetRequiredService<SqlConnection>();
    conn.Open();
    return conn.BeginTransaction();
});

builder.Services.AddScoped<IDbConnection>(s =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSignalR();
builder.Services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();

for (int i = 0; i < 50; i++)
{
    Console.WriteLine(builder.Configuration.GetConnectionString("RabbitMQConnection"));
    
}

Console.WriteLine(builder.Environment.EnvironmentName);
Console.WriteLine(builder.Environment.EnvironmentName);
Console.WriteLine(builder.Environment.EnvironmentName);

builder.Services.AddMassTransit(x =>
{
    
    x.AddConsumer<UserRegistrationConsumer>();
    x.AddConsumer<UserUpdateConsumer>();
    x.AddConsumer<UserDeleteConsumer>();
  
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQConnection"));
  
        cfg.ReceiveEndpoint("chat-user-queue", ep =>
        {
            ep.PrefetchCount = 20;
            ep.ConfigureConsumer<UserRegistrationConsumer>(context);
            ep.ConfigureConsumer<UserUpdateConsumer>(context);
            ep.ConfigureConsumer<UserDeleteConsumer>(context);
        });
    });
});

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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .WithOrigins("http://localhost:3000")
    .WithOrigins("http://localhost:3001")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

#region helper
void ConfigureLogger()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticsearchSinkOptions(configuration, env))
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticsearchSinkOptions(IConfiguration configuration, string env)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower()}-{env.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"

    };
}


#endregion
