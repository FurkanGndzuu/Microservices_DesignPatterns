using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public interface IStockReservedRequestPaymentEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public PaymentMessage payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string BuyerId { get; set; }
    }
}
