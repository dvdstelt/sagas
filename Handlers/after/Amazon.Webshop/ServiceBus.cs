using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Messages.Commands;
using NServiceBus;

namespace Amazon.Webshop
{
    class ServiceBus
    {
        private readonly ISendOnlyBus bus;

        public ServiceBus()
        {
            BusConfiguration configuration = new BusConfiguration();
            configuration.UseTransport<MsmqTransport>();
            configuration.UseSerialization<JsonSerializer>();

            var conventions = configuration.Conventions();
            conventions.DefiningCommandsAs(f => f.Namespace != null && f.Namespace.StartsWith("Amazon.Messages") && f.FullName.EndsWith("Command"));
            conventions.DefiningEventsAs(f => f.Namespace != null && f.Namespace.StartsWith("Amazon.Messages") && f.FullName.EndsWith("Event"));

            bus = Bus.CreateSendOnly(configuration);
        }

        public void AddMovieToCart(Guid cartId, Guid movieId)
        {
            var cmd = new AddItemToCartCommand
            {
                CartId = cartId,
                ProductId = movieId
            };

            bus.Send(cmd);
        }

        public void CancelCart(Guid cartId)
        {
            var cmd = new CancelCartCommand
            {
                CartId = cartId
            };

            bus.Send(cmd);
        }

        public void FinalizeCart(Guid cartId)
        {
            var cmd = new PayCartCommand()
            {
                CartId =  cartId
            };

            bus.Send(cmd);
        }
    }
}
