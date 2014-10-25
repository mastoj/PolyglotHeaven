using System;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Contracts.Types;
using PolyglotHeaven.Domain.Exceptions;
using PolyglotHeaven.Tests;
using NUnit.Framework;

namespace PolyglotHeaven.Domain.Tests.BasketTests
{
    [TestFixture]
    public class CheckoutBasketTests : TestBase
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void WhenTheUserCheckoutWithInvalidAddress_IShouldGetNotified(string street)
        {
            var address = street == null ? null : new Address(street);
            var id = Guid.NewGuid();
            Given(new BasketCreated(id, Guid.NewGuid(), 0));
            WhenThrows<MissingAddressException>(new CheckoutBasket(id, address));
        }

        [Test]
        public void WhenTheUserCheckoutWithAValidAddress_IShouldProceedToTheNextStep()
        {
            var address = new Address("Valid street");
            var id = Guid.NewGuid();
            Given(new BasketCreated(id, Guid.NewGuid(), 0));
            When(new CheckoutBasket(id, address));
            Then(new BasketCheckedOut(id, address));
        }
    }
}