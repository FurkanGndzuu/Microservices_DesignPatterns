using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Consts
{
    public class RabbitMQSetttings
    {
        public const  string SagaOrder = "saga-order-queue";
        public const string StockOrderCreatedEventConsumer = "saga-stock-order-created-qeueu";
        public const string PaymentStockReservedEvent = "saga-payment-stock-reserved-qeueu";
        public const string OrderCompletedEventQueue = "saga-order-completed-qeueu";
        public const string OrderFailedEventQueue = "saga-order-failed-qeueu";
        public const string StockRollBackMessageQueue = "saga-stock-roll-back-message-queue";
    }
}
