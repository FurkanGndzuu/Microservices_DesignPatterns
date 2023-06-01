using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCompletedRequestEventConsumer>();
    x.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["RabbitMQ:Host"], "/", x => {

            x.Username(builder.Configuration["RabbitMQ:username"]);
            x.Password(builder.Configuration["RabbitMQ:password"]);
        });

        configuration.ReceiveEndpoint(RabbitMQSetttings.OrderCompletedEventQueue , e =>
        {
            e.ConfigureConsumer<OrderCompletedRequestEventConsumer>(context);
        });
    });
});

builder.Services.AddOptions<MassTransitHostOptions>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
}, ServiceLifetime.Scoped);

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
