using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Messages.Commands;
using Amazon.Messages.Events;
using NServiceBus;
using NServiceBus.Saga;
using NServiceBus.SagaPersisters.NHibernate;

namespace Amazon.Store.Sagas
{
    public class ShoppingCartSagaData : IContainSagaData
    {
        public ShoppingCartSagaData()
        {
            Items = new List<Item>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Originator { get; set; }
        public virtual string OriginalMessageId { get; set; }

        [RowVersion]
        public virtual int RowVersion { get; set; }

        [Unique]
        public virtual Guid CartId { get; set; }

        public virtual IList<Item> Items { get; set; }
    }

    public class Item
    {
        public virtual Guid ProductId { get; set; }
    }

    public class ShoppingCartSaga : Saga<ShoppingCartSagaData>,
        IAmStartedByMessages<AddItemToCartCommand>,
        IHandleMessages<CancelCartCommand>,
        IHandleMessages<PayCartCommand>,
        IHandleTimeouts<ShoppingCartSaga.BuyersRemorseTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShoppingCartSagaData> mapper)
        {
            mapper.ConfigureMapping<AddItemToCartCommand>(m => m.CartId).ToSaga(s => s.CartId);
            mapper.ConfigureMapping<CancelCartCommand>(m => m.CartId).ToSaga(s => s.CartId);
            mapper.ConfigureMapping<PayCartCommand>(m => m.CartId).ToSaga(s => s.CartId);
        }

        public void Handle(AddItemToCartCommand message)
        {
            Data.CartId = message.CartId;
            Data.Items.Add(new Item() { ProductId = message.ProductId });
        }


        public void Handle(CancelCartCommand message)
        {
            MarkAsComplete();
        }

        public void Handle(PayCartCommand message)
        {
            RequestTimeout(TimeSpan.FromSeconds(10), new BuyersRemorseTimeout());
        }

        public class BuyersRemorseTimeout
        {
            public DateTime RaisedAtUtc { get; set; } = DateTime.UtcNow;
        }

        public void Timeout(BuyersRemorseTimeout state)
        {
            var msg = new OrderFinalizedEvent();
            msg.CartId = Data.CartId;
            msg.Products = new List<Guid>();
            msg.Products.AddRange(Data.Items.Select(s => s.ProductId));

            Bus.Publish(msg);

            MarkAsComplete();
        }
    }
}
