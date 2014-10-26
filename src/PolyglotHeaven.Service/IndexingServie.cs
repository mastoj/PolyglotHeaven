using System;
using System.Collections.Generic;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Service.Documents;
using EventStore.ClientAPI;
using Neo4jClient;

namespace PolyglotHeaven.Service
{
    internal class IndexingServie
    {
        private Indexer _indexer;
        private Dictionary<Type, Action<object>> _eventHandlerMapping;
        private Position? _latestPosition;
        private IEventStoreConnection _connection;
        private GraphClient _graphClient;

        public void Start()
        {
            _graphClient = CreateGraphClient();
            _indexer = CreateIndexer();
            _eventHandlerMapping = CreateEventHandlerMapping();
            ConnectToEventstore();
        }

        private GraphClient CreateGraphClient()
        {
            var graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
            graphClient.Connect();
            DeleteAll(graphClient);
            return graphClient;
        }

        private void DeleteAll(GraphClient graphClient)
        {
            graphClient.Cypher.Match("(n)")
                .OptionalMatch("(n)-[r]-()")
                .Delete("n,r")
                .ExecuteWithoutResults();
        }

        private Indexer CreateIndexer()
        {
            var indexer = new Indexer();
            indexer.Init();
            return indexer;
        }

        private void ConnectToEventstore()
        {

            _latestPosition = Position.Start;
            _connection = EventStoreConnectionWrapper.Connect();
            _connection.Connected +=
                (sender, args) => _connection.SubscribeToAllFrom(_latestPosition, false, HandleEvent);
            Console.WriteLine("Indexing service started");
        }

        private void HandleEvent(EventStoreCatchUpSubscription arg1, ResolvedEvent arg2)
        {
            var @event = EventSerialization.DeserializeEvent(arg2.OriginalEvent);
            if (@event != null)
            {
                var eventType = @event.GetType();
                if (_eventHandlerMapping.ContainsKey(eventType))
                {
                    _eventHandlerMapping[eventType](@event);
                }
            }
            _latestPosition = arg2.OriginalPosition;
        }

        private Dictionary<Type, Action<object>> CreateEventHandlerMapping()
        {
            return new Dictionary<Type, Action<object>>()
            {
                {typeof (CustomerCreated), o => Handle(o as CustomerCreated)},
                {typeof (ProductCreated), o => Handle(o as ProductCreated)}
            }; 
        }

        private void Handle(OrderPlaced evt)
        {
            //_graphClient.Cypher
            //    .Match("(customer:Customer)-[:ORDERED]->(order:Basket)-[]->(product:Product)")
            //    .Where((Order order) => order.Id == evt.BasketId)
            //    .Create("customer-[:BOUGHT]->product")
            //    .ExecuteWithoutResults();
        }

        private void Handle(ProductCreated evt)
        {
            var product = new Product()
            {
                Id = evt.Id,
                Name = evt.Name,
                Price = evt.Price
            };
            _indexer.Index(product);
            _graphClient.Cypher
                .Create("(product:Product {newProduct})")
                .WithParam("newProduct", product)
                .ExecuteWithoutResults();
        }

        private void Handle(CustomerCreated evt)
        {
            var customer = new Customer()
            {
                Id = evt.Id,
                Name = evt.Name
            };
            _indexer.Index(customer);

            _graphClient.Cypher
                .Create("(customer:Customer {newCustomer})")
                .WithParam("newCustomer", customer)
                .ExecuteWithoutResults(); 
        }

        public void Stop()
        {
        }
    }
}