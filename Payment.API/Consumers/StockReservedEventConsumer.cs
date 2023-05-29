using MassTransit;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly ILogger<StockReservedEventConsumer> _logger;

        public StockReservedEventConsumer(IPublishEndpoint endpoint, ILogger<StockReservedEventConsumer> logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000;

            if(balance >= context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for user id= {context.Message.BuyerId}");

                await _endpoint.Publish<PaymentSucceesedEvent>(new PaymentSucceesedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was not withdrawn from credit card for user id={context.Message.BuyerId}");

                await _endpoint.Publish(new PaymentFailedEvent()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId  = context.Message.BuyerId,
                    Message = "not enough balance",
                    OrderItems = context.Message.OrderItems,
                });
            }
        }
    }
}
