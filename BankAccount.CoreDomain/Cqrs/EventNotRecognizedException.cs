using System;

namespace BankAccount.CoreDomain.Cqrs
{
    public class EventNotRecognizedException : Exception
    {
        public EventNotRecognizedException()
        {
        }

        public EventNotRecognizedException(string message)
            : base(message)
        {
        }

        public EventNotRecognizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}