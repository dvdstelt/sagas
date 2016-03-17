using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Messages.Events;
using NServiceBus;

namespace Amazon.Store.Handlers
{
    class OrderFinalizedHandler : IHandleMessages<OrderFinalizedEvent>
    {
        public void Handle(OrderFinalizedEvent message)
        {
            Console.WriteLine($"Received {message.Products.Count} products for cart {message.CartId}");
        }
    }
}
