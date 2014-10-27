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

        public void AddOrder(OrderPlaced evt)
        {
            GraphOrder newOrder = new GraphOrder(evt.Id);
            _graphClient.Cypher
                .Match("(customer:Customer)")
                .Where((Customer customer) => customer.Id == evt.CustomerId)
                .Create("customer-[:PLACED_ORDER]->(order: Order {order})")
                .WithParam("order", newOrder)
                .ExecuteWithoutResults();

            foreach (var orderItem in evt.Items)
            {
                _graphClient.Cypher
                    .Match("(product:Product)", "(order:Order)")
                    .Where((Product product) => product.Id == orderItem.ProductId)
                    .AndWhere((GraphOrder order) => order.Id == newOrder.Id)
                    .Create("order-[:INCLUDES {Quantity: {Quantity}}]->product")
                    .WithParam("Quantity", orderItem.Quantity)
                    .ExecuteWithoutResults();

            }
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