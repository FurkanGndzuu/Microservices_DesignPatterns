using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Consts;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockDbContext _context;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEndpoint;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(StockDbContext context, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpoint, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _sendEndpoint = sendEndpoint;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();

            foreach(var orderItem in context.Message.orderItems)
            {
                stockResult.Add(await _context.Stocks.AnyAsync(x => x.ProductId == orderItem.ProductId && x.Count > orderItem.Count));
            }

            if(stockResult.All(x => x.Equals(true)))
            {
                foreach(var item in context.Message.orderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Stock was reserved for Buyer Id :{context.Message.BuyerId}");


                var sendEndpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StockReservedEventConsumerQueue}"));

                StockReservedEvent stockReservedEvent = new StockReservedEvent()
                {
                    Payment = context.Message.Payment,
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.orderItems
                };

                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Not enough stock"
                });

                _logger.LogInformation($"Not enough stock for Buyer Id :{context.Message.BuyerId}");

            }

        }

    }
}
