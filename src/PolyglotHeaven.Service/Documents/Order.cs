using System;
using Nest;

namespace PolyglotHeaven.Service.Documents
{
    public class Order
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid Id { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid OrderId { get; set; }
    }
}