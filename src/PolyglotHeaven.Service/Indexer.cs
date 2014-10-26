using System;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Service.Documents;
using Nest;

namespace PolyglotHeaven.Service
{
    internal class Indexer
    {
        private readonly IElasticClient _esClient;
        private string _index = "PolyglotHeaven";

        public Indexer()
        {
            _esClient = ElasticClientBuilder.BuildClient();
        }

        public TDocument Get<TDocument>(Guid id) where TDocument : class
        {
            return _esClient.Get<TDocument>(id.ToString()).Source;
        }

        public void Index<TDocument>(TDocument document) where TDocument : class
        {
            _esClient.Index(document, y => y.Index(_index));
        }

        public void Init()
        {
            _esClient.CreateIndex(_index, y => y
                .AddMapping<Customer>(m => m.MapFromAttributes())
                .AddMapping<Product>(m => m.MapFromAttributes()));
        }
    }
}