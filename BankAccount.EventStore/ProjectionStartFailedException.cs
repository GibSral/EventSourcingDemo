using System;

namespace BankAccount.EventStore
{
    public class ProjectionStartFailedException : Exception
    {
        public ProjectionStartFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}