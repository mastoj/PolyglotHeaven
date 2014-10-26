using System;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Domain.Aggregates;
using PolyglotHeaven.Domain.Exceptions;
using PolyglotHeaven.Infrastructure;
using PolyglotHeaven.Infrastructure.Exceptions;

namespace PolyglotHeaven.Domain.CommandHandlers
{
    internal class CustomerCommandHandler : 
        IHandle<CreateCustomer>
    {
        private readonly IDomainRepository _domainRepository;

        public CustomerCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IAggregate Handle(CreateCustomer command)
        {
            try
            {
                var customer = _domainRepository.GetById<Customer>(command.Id);
                throw new CustomerAlreadyExistsException(command.Id);
            }
            catch (AggregateNotFoundException)
            {
                // We expect not to find anything
            }
            return Customer.Create(command.Id, command.Name);
        }
    }
}