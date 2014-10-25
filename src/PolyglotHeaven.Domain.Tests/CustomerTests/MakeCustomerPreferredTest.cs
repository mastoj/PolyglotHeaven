using System;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Tests;
using NUnit.Framework;

namespace PolyglotHeaven.Domain.Tests.CustomerTests
{
    [TestFixture]
    public class MarkCustomerAsPreferredTest : TestBase
    {
        [TestCase(25)]
        [TestCase(50)]
        [TestCase(70)]
        public void GivenTheUserExists_WhenMarkingCustomerAsPreferred_ThenTheCustomerShouldBePreferred(int discount)
        {
            Guid id = Guid.NewGuid();
            Given(new CustomerCreated(id, "Superman"));
            When(new MarkCustomerAsPreferred(id, discount));
            Then(new CustomerMarkedAsPreferred(id, discount));
        }
    }
}