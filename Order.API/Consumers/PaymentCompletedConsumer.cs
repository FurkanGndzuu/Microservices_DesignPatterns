using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentCompletedConsumer : IConsumer<PaymentSucceesedEvent>
    {
        readonly AppDbContext _appDbContext;
        readonly ILogger<PaymentCompletedConsumer> _logger;

        public PaymentCompletedConsumer(ILogger<PaymentCompletedConsumer> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentSucceesedEvent> context)
        {
            var order =await _appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);

            if(order is not null)
            {
                order.Status = OrderStatus.Complete;
               await _appDbContext.SaveChangesAsync();
                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
