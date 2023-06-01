using MassTransit;
using Payment.API.Consumers;
using Shared.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(opt =>
{

    opt.AddConsumer<PaymentStockReservedEventConsumer>();
    opt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", x =>
        {
            x.Username("guest");
            x.Password("guest");
        });
        cfg.ReceiveEndpoint(RabbitMQSetttings.PaymentStockReservedEvent, e =>
        {
            e.ConfigureConsumer<PaymentStockReservedEventConsumer>(context);
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
