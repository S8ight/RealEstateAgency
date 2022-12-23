using System.Data;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using REA.ChatSystem.BLL.Hubs;
using REA.ChatSystem.BLL.Interfaces;
using REA.ChatSystem.BLL.Services;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped((s) => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AgencyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


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

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserConsumer>();
 
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://rabbitmq:5672");
 
        cfg.ReceiveEndpoint("chat-user-queue", ep =>
        {
            ep.PrefetchCount = 20;
            ep.ConfigureConsumer<UserConsumer>(context);
        });
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

