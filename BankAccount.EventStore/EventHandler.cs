using System;

namespace BankAccount.EventStore
{
    public class EventHandler
    {
        public EventHandler(string eventType, Action<object> handler, Action<Exception, object> onError)
        {
            EventType = eventType;
            Handler = handler;
            OnError = onError;
        }

        public string EventType { get; }

        public Action<object> Handler { get; }

        public Action<Exception, object> OnError { get; }
    }
}