using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockReservedRequestPaymentEvent : IStockReservedRequestPaymentEvent
    {
        public Guid CorrelationId { get; }
        public PaymentMessage payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string BuyerId { get; set; }

        public StockReservedRequestPaymentEvent(Guid correlation)
        {
            CorrelationId = correlation;
        }
    }
}
