using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Domain.Aggregates;
using PolyglotHeaven.Domain.Services;
using PolyglotHeaven.Infrastructure;

namespace PolyglotHeaven.Domain.CommandHandlers
{
    internal class OrderCommandHandler :
        IHandle<PlaceOrder>
    {
        private readonly IDomainRepository _domainRepository;

        public OrderCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IAggregate Handle(PlaceOrder command)
        {
            var lookup = new LookupAggregate(_domainRepository);
            var order = Order.Create(command.Id, command.CustomerId, command.Items, lookup);
            return order;
        }
    }
}