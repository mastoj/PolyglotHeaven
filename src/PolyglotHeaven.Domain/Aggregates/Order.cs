using System;
using PolyglotHeaven.Domain.Exceptions;
using PolyglotHeaven.Infrastructure;
using Microsoft.FSharp.Collections;

namespace PolyglotHeaven.Domain.Aggregates
{
    internal class Order : AggregateBase
    {
        public Order()
        {
        }
    }
}