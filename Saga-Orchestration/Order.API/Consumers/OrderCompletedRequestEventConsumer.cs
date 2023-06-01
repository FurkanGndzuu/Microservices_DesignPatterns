using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Abstractions;

namespace Order.API.Consumers
{
    public class OrderCompletedRequestEventConsumer : IConsumer<IOrderCompletedRequestEvent>
    {
        private readonly AppDbContext _context;

        private readonly ILogger<OrderCompletedRequestEventConsumer> _logger;

        public OrderCompletedRequestEventConsumer(AppDbContext context, ILogger<OrderCompletedRequestEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IOrderCompletedRequestEvent> context)
        {
           Models.Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if(order is not null)
            {
                order.Status = OrderStatus.Complete;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
