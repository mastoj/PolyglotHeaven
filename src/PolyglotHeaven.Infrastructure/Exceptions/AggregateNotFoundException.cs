using System;

namespace PolyglotHeaven.Infrastructure.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(string message)
            : base(message)
        {
        }
    }
}