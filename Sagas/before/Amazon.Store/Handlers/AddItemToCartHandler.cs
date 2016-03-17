using System;
using System.Linq;
using Amazon.Messages.Commands;
using Amazon.Store.Data.Context;
using Amazon.Store.Data.Models;
using NServiceBus;

namespace Amazon.Store.Handlers
{
    class AddItemToCartHandler : IHandleMessages<AddItemToCartCommand>
    {
        public void Handle(AddItemToCartCommand message)
        {
            AmazonContext ctx = new AmazonContext();
            var cart = ctx.ShoppingCarts.Where(s => s.CartId == message.CartId).FirstOrDefault();

            if (cart == null)
            {
                cart = new ShoppingCart() {CartId = message.CartId};
                ctx.ShoppingCarts.Add(cart);
            }

            ShoppingCartItem item = new ShoppingCartItem() { Id = Guid.NewGuid(), ProductId = message.ProductId, ShoppingCart = cart};
            ctx.ShoppingCartItems.Add(item);
            ctx.SaveChanges();
        }
    }
}
