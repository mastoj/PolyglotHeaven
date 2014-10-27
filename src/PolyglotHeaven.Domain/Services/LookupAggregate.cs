using System;
using PolyglotHeaven.Infrastructure;

namespace PolyglotHeaven.Domain.Services
{
    internal class LookupAggregate
    {
        private readonly IDomainRepository _domainRepository;

        public LookupAggregate(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : IAggregate, new()
        {
            return _domainRepository.GetById<TAggregate>(id);
        }
    }
}