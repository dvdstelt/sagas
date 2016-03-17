using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Messages.Commands;
using Amazon.Store.Data.Context;
using NServiceBus;

namespace Amazon.Store.Handlers
{
    class PayCartHandler : IHandleMessages<PayCartCommand>
    {
        public void Handle(PayCartCommand message)
        {
            AmazonContext ctx = new AmazonContext();
            var cart = ctx.ShoppingCarts.Where(s => s.CartId == message.CartId).FirstOrDefault();

            cart.IsPaid = true;
            ctx.SaveChanges();
        }
    }
}
