using System;

namespace PolyglotHeaven.Domain.Exceptions
{
    public class CustomerAlreadyExistsException : DuplicateAggregateException
    {
        public CustomerAlreadyExistsException(Guid id) : base(id)
        {
            
        }
    }
}