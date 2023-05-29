using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Consts;
using Stock.API.Consumers;
using Stock.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<StockDbContext>(opt =>
{
    opt.UseInMemoryDatabase("StockDb");
},ServiceLifetime.Scoped);

builder.Services.AddMassTransit(opt =>
{

    opt.AddConsumer<OrderCreatedEventConsumer>();
    opt.AddConsumer<PaymentFailedEventConsumer>();

    opt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", x =>
        {
            x.Username("guest");
            x.Password("guest");

        });

        cfg.ReceiveEndpoint(RabbitMQSettings.StockOrderCreatedEventConsumerQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(RabbitMQSettings.StockPaymentFailedEventQueue, e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });
    });
});

builder.Services.AddOptions<MassTransitHostOptions>();

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
