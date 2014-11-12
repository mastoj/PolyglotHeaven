using System;
using System.Collections.Generic;
using PolyglotHeaven.Contracts.Events;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Infrastructure;
using PolyglotHeaven.Service.Documents;
using EventStore.ClientAPI;
using Neo4jClient;

namespace PolyglotHeaven.Service
{
    internal class IndexingServie
    {
        private Indexer _indexer;
        private Position? _latestPosition;
        private IEventStoreConnection _connection;
        private GraphtIndexer _graphIndexer;
        private ActionRouter _actionRouter;

        public void Start()
        {
            _indexer = CreateIndexer();
            _graphIndexer = CreateGraphIndexer();
            _actionRouter = CreateActionRouter();
            ConnectToEventstore();
        }

        private ActionRouter CreateActionRouter()
        {
            var actionRouter = new ActionRouter();
            actionRouter.Add<CustomerCreated>(Handle);
            actionRouter.Add<ProductCreated>(Handle);
            return actionRouter;
        }

        private GraphtIndexer CreateGraphIndexer()
        {
            var indexer = new GraphtIndexer();
            indexer.Init();
            return indexer;
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
                _actionRouter.Execute(@event);
            }
            _latestPosition = arg2.OriginalPosition;
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
            _graphIndexer.AddProduct(product);
        }

        private void Handle(CustomerCreated evt)
        {
            var customer = new Customer()
            {
                Id = evt.Id,
                Name = evt.Name
            };
            _indexer.Index(customer);
            _graphIndexer.AddCustomer(customer);
        }

        public void Stop()
        {
        }
    }
}