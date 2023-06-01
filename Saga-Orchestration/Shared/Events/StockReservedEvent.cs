using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public Guid CorrelationId { get; }

        public List<OrderItemMessage> OrderItems { get; set; }

        public StockReservedEvent(Guid correlation)
        {
            CorrelationId = correlation;
        }
    }
}
