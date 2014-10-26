using System;
using Nest;

namespace PolyglotHeaven.Service.Documents
{
    public class Order
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid Id { get; set; }
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid ProductId { get; set; }

        public int Price { get; set; }
        public int Discount { get; set; }
        public int DiscountedPrice { get; set; }
    }
}