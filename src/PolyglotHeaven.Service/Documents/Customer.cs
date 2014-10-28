using System;
using Nest;

namespace PolyglotHeaven.Service.Documents
{
    public class Customer
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public Guid Id { get; set; }
        [ElasticProperty(Analyzer = "my_edge_ngram_analyzer")]
        public string Name { get; set; }
        public bool IsPreferred { get; set; }
        public int Discount { get; set; }
    }
}