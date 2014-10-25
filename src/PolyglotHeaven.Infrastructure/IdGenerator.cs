using System;

namespace PolyglotHeaven.Infrastructure
{
    public class IdGenerator
    {
        private static Func<Guid> _generator;

        public static Func<Guid> GenerateGuid
        {
            get
            {
                _generator = _generator ?? Guid.NewGuid;
                return _generator;
            }
            set { _generator = value; }
        }
    }
}