using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared.Consts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, configuration) =>
    {
        x.AddConsumer<PaymentCompletedConsumer>();
        x.AddConsumer<PaymentFailedEventConsumer>();
        x.AddConsumer<StockNotReservedConsumer>();
        configuration.Host(builder.Configuration["RabbitMQ:Host"],"/", x => {
            
            x.Username(builder.Configuration["RabbitMQ:username"]);
            x.Password(builder.Configuration["RabbitMQ:password"]);
 
        });

        configuration.ReceiveEndpoint(RabbitMQSettings.OrderPaymentCompletedEventQueue, e =>
        {
            e.ConfigureConsumer<PaymentCompletedConsumer>(context);
        });
        configuration.ReceiveEndpoint(RabbitMQSettings.OrderPaymentFailedEventQueue, e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });
        configuration.ReceiveEndpoint(RabbitMQSettings.OrderStockNotReservedEventQueue, e =>
        {
            e.ConfigureConsumer<StockNotReservedConsumer>(context);
        });
    });
});

builder.Services.AddOptions<MassTransitHostOptions>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
},ServiceLifetime.Scoped);

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
