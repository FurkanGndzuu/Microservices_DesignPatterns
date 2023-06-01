using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Messages;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class StockRollBackConsumer : IConsumer<IStockRollBackMessage>
    {
        private readonly StockDbContext _context;
        private ILogger<StockRollBackConsumer> _logger;

        public StockRollBackConsumer(StockDbContext context, ILogger<StockRollBackConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IStockRollBackMessage> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Stock was released");
        }
    }
}
