using System;
using NUnit.Framework;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Contracts.Types;

namespace PolyglotHeaven.Tests.OrderTests
{
    [TestFixture]
    public class PlaceOrderTests : TestBase
    {
        [Test]
        public void PlaceOrder_Should_CreateAnOrder()
        {
            var productId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var orderItems = Contracts.Helpers.ToFSharpList(new[] {new OrderItem(productId, 20)});
            Given(new ProductCreated(productId, "a name", 20), new CustomerCreated(customerId, "John Doe"));
            When(new PlaceOrder(orderId, customerId, orderItems));
            Then(new OrderPlaced(orderId, customerId, orderItems));
        }
    }
}