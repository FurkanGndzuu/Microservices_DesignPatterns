using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Consts
{
    public class RabbitMQSettings
    {
        public const string StockOrderCreatedEventConsumerQueue = "Stock_OrderCreated";
        public const string StockReservedEventConsumerQueue = "Stock_Reserved";
        public const string StockNotReservedEventConsumerQueue = "Stock_NotReserved";
        public const string OrderPaymentCompletedEventQueue = "Order_PaymentCompleted";
        public const string OrderPaymentFailedEventQueue = "Order_PaymentFailed";
        public const string OrderStockNotReservedEventQueue = "Order_StockNotReserved";
        public const string StockPaymentFailedEventQueue = "Stock_PaymentFailed";
    }
}
