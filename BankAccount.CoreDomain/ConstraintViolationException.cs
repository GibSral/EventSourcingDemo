using System;

namespace BankAccount.CoreDomain
{
    public class ConstraintViolationException : System.Exception
    {
        public ConstraintViolationException(string message)
            : base(message)
        {
        }

        public ConstraintViolationException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}