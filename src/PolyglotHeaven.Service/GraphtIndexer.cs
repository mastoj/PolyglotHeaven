using System;
using Neo4jClient;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Service.Documents;

namespace PolyglotHeaven.Service
{
    internal class GraphtIndexer
    {
        private GraphClient _graphClient;

        public GraphtIndexer()
        {
            _graphClient = Neo4jClientBuilder.Build();
        }

        public void AddProduct(Product product)
        {
            _graphClient.Cypher
                .Create("(product:Product {newProduct})")
                .WithParam("newProduct", product)
                .ExecuteWithoutResults();
        }

        private void DeleteAll(GraphClient graphClient)
        {
            graphClient.Cypher.Match("(n)")
                .OptionalMatch("(n)-[r]-()")
                .Delete("n,r")
                .ExecuteWithoutResults();
        }

        public void Init()
        {
            DeleteAll(_graphClient);
        }

        public void AddCustomer(Customer customer)
        {
            _graphClient.Cypher
                .Create("(customer:Customer {newCustomer})")
                .WithParam("newCustomer", customer)
                .ExecuteWithoutResults();
        }
    }

    public class GraphOrder
    {
        public Guid Id { get; set; }

        public GraphOrder(Guid id)
        {
            Id = id;
        }
    }
}