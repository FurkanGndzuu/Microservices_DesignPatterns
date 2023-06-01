using MassTransit;
using Shared.Abstractions;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class PaymentStockReservedEventConsumer : IConsumer<IStockReservedRequestPaymentEvent>
    {
        private readonly ILogger<PaymentStockReservedEventConsumer> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public PaymentStockReservedEventConsumer(ILogger<PaymentStockReservedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestPaymentEvent> context)
        {
            var balance = 3000m;

            if (balance > context.Message.payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.payment.TotalPrice} TL was withdrawn from credit card for user id= {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation($"{context.Message.payment.TotalPrice} TL was not withdrawn from credit card for user id={context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId) { FailMessage = "not enough balance", OrderItems = context.Message.OrderItems });
            }

        }
    }
}
