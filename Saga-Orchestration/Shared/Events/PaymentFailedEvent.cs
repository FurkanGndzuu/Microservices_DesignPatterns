using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public Guid CorrelationId { get; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string FailMessage { get; set; }

        public PaymentFailedEvent(Guid Correlation)
        {
            CorrelationId = Correlation;
        }
    }
}
