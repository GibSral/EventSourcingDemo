using System;

namespace BankAccount.EventStore
{
    public interface ILog
    {
        bool IsDebugEnabled { get; }

        void Debug(string message);

        void Debug(Exception exception);

        void Info(string message);

        void Warn(string message);

        void Error(string message);

        void Error(Exception exception);

        void Error(string message, Exception exception);

        void Critical(string message);
    }
}