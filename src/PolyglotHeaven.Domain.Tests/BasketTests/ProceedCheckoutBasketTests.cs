using System;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Tests;
using NUnit.Framework;

namespace PolyglotHeaven.Domain.Tests.BasketTests
{
    [TestFixture]
    public class ProceedCheckoutBasketTests : TestBase
    {
        [Test]
        public void GivenABasket_WhenCreatingABasketForCustomerX_ThenTheBasketShouldBeCreated()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            int discount = 0;
            Given(new BasketCreated(id, customerId, discount));
            When(new ProceedToCheckout(id));
            Then(new CustomerIsCheckingOutBasket(id));
        }
    }
}