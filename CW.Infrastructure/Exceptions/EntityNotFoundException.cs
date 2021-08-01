using System;

namespace CW.Infrastructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message): base(message) { }
    }
}
