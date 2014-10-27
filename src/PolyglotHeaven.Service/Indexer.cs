using System;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Service.Documents;
using Nest;

namespace PolyglotHeaven.Service
{
    internal class Indexer
    {
        private readonly IElasticClient _esClient;

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
//            _esClient.Index(document, y => y.Index(ElasticClientBuilder.IndexName));
        }

        public void Init()
        {
            _esClient.DeleteIndex(did => did.Index(ElasticClientBuilder.IndexName));
            _esClient.CreateIndex(cid => cid
                .Index(ElasticClientBuilder.IndexName)
                .AddMapping<Customer>(m => m.MapFromAttributes())
                .AddMapping<Product>(m => m.MapFromAttributes()));
        }


        private static void SetupAnalyzer(IElasticClient esClient)
        {
            esClient.UpdateSettings(usd => usd
                .Analysis(ad => ad
                    .Tokenizers(fd => fd
                        .Add("my_edge_ngram_tokenizer", new EdgeNGramTokenizer()
            {
                MaxGram = 8,
                MinGram = 2,
                TokenChars = new[] { "letter", "digit" }
            }))
                    .Analyzers(fd => fd
                        .Add("my_edge_ngram_analyzer", new CustomAnalyzer("my_edge_ngram_tokenizer")))));
        }
    }
}