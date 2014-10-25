using System;
using PolyglotHeaven.Contracts.Types;
using Nest;

namespace PolyglotHeaven.Service.Documents
{
    public class Basket
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid Id { get; set; }
        [ElasticProperty(Type = FieldType.Nested)]
        public OrderLine[] OrderLines { get; set; }
        public BasketState BasketState { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid OrderId { get; set; }
    }

    public enum BasketState
    {
        Shopping,
        CheckingOut,
        CheckedOut,
        Paid
    }
}