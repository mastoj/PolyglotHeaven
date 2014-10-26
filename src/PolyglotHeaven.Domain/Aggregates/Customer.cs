using System;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Infrastructure;

namespace PolyglotHeaven.Domain.Aggregates
{
    internal class Customer : AggregateBase
    {
        public Customer()
        {
            RegisterTransition<CustomerCreated>(Apply);
        }

        private Customer(Guid id, string name) : this()
        {
            RaiseEvent(new CustomerCreated(id, name));
        }

        internal int Discount { get; set; }

        private void Apply(CustomerCreated obj)
        {
            Id = obj.Id;
        }

        internal static IAggregate Create(Guid id, string name)
        {
            return new Customer(id, name);
        }
    }
}