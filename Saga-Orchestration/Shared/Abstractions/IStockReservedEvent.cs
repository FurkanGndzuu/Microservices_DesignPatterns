using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }
        public List<OrderItemMessage> OrderItems { get; set; }

    }
}
