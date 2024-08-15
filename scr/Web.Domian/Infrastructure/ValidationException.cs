using System;

namespace Web.Domian.Infrastructure
{
    public class ValidationException : Exception
    {
        public object Payload { get; }

        public ValidationException(string message, object payload = null) : base(message)
        {
            Payload = payload;
        }
    }
}
