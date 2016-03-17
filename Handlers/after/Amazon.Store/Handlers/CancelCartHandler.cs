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
    class CancelCartHandler : IHandleMessages<CancelCartCommand>
    {
        public void Handle(CancelCartCommand message)
        {
            AmazonContext ctx = new AmazonContext();
            var cart = ctx.ShoppingCarts.FirstOrDefault(s => s.CartId == message.CartId);

            ctx.ShoppingCarts.Remove(cart);
            ctx.SaveChanges();
        }
    }
}
