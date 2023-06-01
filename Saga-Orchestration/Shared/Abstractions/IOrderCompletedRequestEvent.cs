using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions
{
    public interface IOrderCompletedRequestEvent
    {
        public int OrderId { get; set; }
    }
}
