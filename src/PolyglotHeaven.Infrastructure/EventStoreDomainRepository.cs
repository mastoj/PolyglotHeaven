using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolyglotHeaven.Infrastructure.Exceptions;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace PolyglotHeaven.Infrastructure
{
    public class EventStoreDomainRepository : DomainRepositoryBase
    {
        private IEventStoreConnection _connection;
        private const string Category = "PolyglotHeaven";

        public EventStoreDomainRepository(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        private string AggregateToStreamName(Guid id)
        {
            return string.Format("{0}-{1}", Category, id);
        }

        public override IEnumerable<IEvent> Save<TAggregate>(TAggregate aggregate)
        {
            var events = aggregate.UncommitedEvents().ToList();
            var expectedVersion = CalculateExpectedVersion(aggregate, events);
            var eventData = events.Select(CreateEventData);
            var streamName = AggregateToStreamName(aggregate.Id);
            _connection.AppendToStreamAsync(streamName, expectedVersion, eventData).Wait();
            return events;
        }

        public override TResult GetById<TResult>(Guid id)
        {
            var streamName = AggregateToStreamName(id);
            var eventsSlice = _connection.ReadStreamEventsForwardAsync(streamName, 0, int.MaxValue, false).Result;
            if (eventsSlice.Status == SliceReadStatus.StreamNotFound)
            {
                throw new AggregateNotFoundException("Could not found aggregate of type " + typeof(TResult) + " and id " + id);
            }
            var deserializedEvents = eventsSlice.Events.Select(e =>
            {
                var metadata = DeserializeObject<Dictionary<string, string>>(e.OriginalEvent.Metadata);
                var eventData = DeserializeObject(e.OriginalEvent.Data, metadata[EventClrTypeHeader]);
                return eventData as IEvent;
            });
            return BuildAggregate<TResult>(deserializedEvents);
        }

        private T DeserializeObject<T>(byte[] data)
        {
            return (T)(DeserializeObject(data, typeof(T).AssemblyQualifiedName));
        }

        private object DeserializeObject(byte[] data, string typeName)
        {
            var jsonString = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject(jsonString, Type.GetType(typeName));
        }

        public EventData CreateEventData(object @event)
        {
            var eventHeaders = new Dictionary<string, string>()
            {
                {
                    EventClrTypeHeader, @event.GetType().AssemblyQualifiedName
                }
            };
            var eventDataHeaders = SerializeObject(eventHeaders);
            var data = SerializeObject(@event);
            var eventData = new EventData(Guid.NewGuid(), @event.GetType().Name, true, data, eventDataHeaders);
            return eventData;
        }

        private byte[] SerializeObject(object obj)
        {
            var jsonObj = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(jsonObj);
            return data;
        }

        public string EventClrTypeHeader = "EventClrTypeName";
    }
}