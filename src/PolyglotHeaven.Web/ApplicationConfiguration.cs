using PolyglotHeaven.Domain;
using PolyglotHeaven.Helpers;
using PolyglotHeaven.Infrastructure;

namespace PolyglotHeaven.Web
{
    public class ApplicationConfiguration
    {
        public static DomainEntry CreateDomainEntry()
        {
            var connection = EventStoreConnectionWrapper.Connect();
            var domainRepository = new EventStoreDomainRepository(connection);
            var domainEntry = new DomainEntry(domainRepository);
            return domainEntry;
        }
    }
}