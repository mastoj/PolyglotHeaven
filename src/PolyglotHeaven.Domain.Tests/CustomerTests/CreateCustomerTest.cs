using System;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Domain.Exceptions;
using PolyglotHeaven.Tests;
using NUnit.Framework;

namespace PolyglotHeaven.Domain.Tests.CustomerTests
{
    [TestFixture]
    public class CreateCustomerTest : TestBase
    {
        [Test]
        public void WhenCreatingTheCustomer_TheCustomerShouldBeCreatedWithTheRightName()
        {
            Guid id = Guid.NewGuid();
            When(new CreateCustomer(id, "Tomas"));
            Then(new CustomerCreated(id, "Tomas"));
        }

        [Test]
        public void GivenAUserWithIdXExists_WhenCreatingACustomerWithIdX_IShouldGetNotifiedThatTheUserAlreadyExists()
        {
            Guid id = Guid.NewGuid();
            Given(new CustomerCreated(id, "Something I don't care about"));
            WhenThrows<CustomerAlreadyExistsException, CreateCustomer>(new CreateCustomer(id, "Tomas"));
        }
    }
}