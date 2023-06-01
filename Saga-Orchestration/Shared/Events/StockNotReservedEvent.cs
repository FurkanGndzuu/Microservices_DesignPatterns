using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {
        public Guid CorrelationId { get; }

        public List<OrderItemMessage> OrderItems { get; set; }
        public string FailMessage { get; set; }

        public StockNotReservedEvent(Guid correlation)
        {
            CorrelationId = correlation;
        }
    }
}
