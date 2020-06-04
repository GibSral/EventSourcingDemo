using System;
using BankAccount.EventStore;

namespace BankAccount.AllAccountsProjection
{
    public class ConsoleLogger : ILog
    {
        public bool IsDebugEnabled { get; }
        public void Debug(string message)
        {
            Console.WriteLine($"DEBUG: {message}");
        }

        public void Debug(Exception exception)
        {
            Console.WriteLine($"DEBUG: {exception.Message}");
        }

        public void Info(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

        public void Warn(string message)
        {
            Console.WriteLine($"WARN: {message}");
        }

        public void Error(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }

        public void Error(Exception exception)
        {
            Console.WriteLine($"ERROR: {exception.Message}");
        }

        public void Error(string message, Exception exception)
        {
            Error(message);
            Error(exception);
        }

        public void Critical(string message)
        {
            Console.WriteLine($"CRITICAL: {message}");
        }
    }
}