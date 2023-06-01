using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Abstractions;

namespace Order.API.Consumers
{
    public class OrderFailedEventConsumer : IConsumer<IOrderFailedEvent>
    {
        private readonly AppDbContext _context;

        private readonly ILogger<OrderFailedEventConsumer> _logger;

        public OrderFailedEventConsumer(AppDbContext context, ILogger<OrderFailedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IOrderFailedEvent> context)
        {
            Models.Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);

            if(order is not null)
            {
                order.Status = OrderStatus.Fail;
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
