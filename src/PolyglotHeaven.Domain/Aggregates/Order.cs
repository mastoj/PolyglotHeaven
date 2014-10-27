using System;
using System.Collections.Generic;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Contracts.Types;
using PolyglotHeaven.Domain.Services;
using PolyglotHeaven.Infrastructure;
using Microsoft.FSharp.Collections;

namespace PolyglotHeaven.Domain.Aggregates
{
    internal class Order : AggregateBase
    {
        public Order()
        {
            RegisterTransition<OrderPlaced>(Handle);
        }

        private void Handle(OrderPlaced obj)
        {
            Id = obj.Id;
        }

        private Order(Guid id, Guid customerId, FSharpList<OrderItem> items) : this()
        {
            RaiseEvent(new OrderPlaced(id, customerId, items));
        }

        public static IAggregate Create(Guid id, Guid customerId, FSharpList<OrderItem> items, LookupAggregate lookup)
        {
            VerifyAggregateExists<Customer>(customerId, lookup);
            VerifyItems(items, lookup);
            return new Order(id, customerId, items);
        }

        private static void VerifyAggregateExists<TAggregate>(Guid aggregateId, LookupAggregate lookup) where TAggregate : IAggregate, new()
        {
            lookup.GetById<TAggregate>(aggregateId);
        }

        private static void VerifyItems(IEnumerable<OrderItem> items, LookupAggregate lookup)
        {
            foreach (var orderItem in items)
            {
                VerifyAggregateExists<Product>(orderItem.ProductId, lookup);
            }
        }
    }
}