using System;

namespace BankAccount.EventStore
{
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
    public sealed class LoggerNullObject : ILog
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
    {
        public static readonly LoggerNullObject Instance = new LoggerNullObject();

        private LoggerNullObject()
        {
            // Nothing to do -> NullObject
        }

        public bool IsDebugEnabled { get; } = false;

        public void Write(string message)
        {
            // Nothing to do -> NullObject
        }

        public void Debug(string message)
        {
            // Nothing to do -> NullObject
        }

        public void Debug(Exception exception)
        {
            // Nothing to do -> NullObject
        }

        public void Info(string message)
        {
            // Nothing to do -> NullObject
        }

        public void Warn(string message)
        {
            // Nothing to do -> NullObject
        }

        public void Error(string message)
        {
            // Nothing to do -> NullObject
        }

        public void Error(Exception exception)
        {
            // Nothing to do -> NullObject
        }

        public void Error(string message, Exception exception)
        {
            // Nothing to do -> NullObject
        }

        public void Critical(string message)
        {
            // Nothing to do -> NullObject
        }
    }
}
