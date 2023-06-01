using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderCompletedEventRequest : IOrderCompletedRequestEvent
    {
        public int OrderId { get ; set ; }
    }
}
