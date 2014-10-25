using System;

namespace PolyglotHeaven.Infrastructure
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
