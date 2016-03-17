using System;
using System.Runtime.Remoting.Messaging;
using Amazon.Messages.Commands;
using Amazon.Messages.Events;
using Amazon.Store.Sagas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NServiceBus.Testing;

namespace Tests.Amazon.Core
{
    [TestClass]
    public class ShoppingCartSagaTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Test.Initialize(c =>
            {
                c.Conventions().DefiningCommandsAs(s => s.Name.EndsWith("Command"));
                c.Conventions().DefiningEventsAs(s => s.Name.EndsWith("Event"));
                c.AssembliesToScan(new[]
                {
                    typeof(ShoppingCartSaga).Assembly,
                    typeof(CancelCartCommand).Assembly
                });
            });
        }

        [TestMethod]
        public void WhenCartIsPaidTimeoutShouldBeSent()
        {

            Test.Saga<ShoppingCartSaga>()
                .ExpectTimeoutToBeSetIn<ShoppingCartSaga.BuyersRemorseTimeout>((timeout, span) => span == TimeSpan.FromSeconds(10))
                .When(s => s.Handle(new PayCartCommand()))
                .ExpectPublish<OrderFinalizedEvent>()
                .WhenSagaTimesOut()
                //.When(s => s.Timeout(new ShoppingCartSaga.BuyersRemorseTimeout())) // This is possible as well, more explicit
                .AssertSagaCompletionIs(true);
        }

        [TestMethod]
        public void WhenCartIsCancelledSagaShouldBeCompleted()
        {
            Test.Saga<ShoppingCartSaga>()
                .When(s => s.Handle(new CancelCartCommand()))
                .AssertSagaCompletionIs(true);
        }
    }
}
