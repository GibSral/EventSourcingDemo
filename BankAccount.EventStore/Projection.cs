using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccount.EventStore
{
    public abstract class Projection : IProjection
    {
        private readonly List<EventHandler> handlers = new List<EventHandler>();

        public abstract string Id { get; }

        protected void When<TEvent>(Action<TEvent> when) => When(when, (exception, @event) => { });

        protected void When<TEvent>(Action<TEvent> when, Action<Exception, TEvent> onError)
        {
            handlers.Add(new EventHandler(typeof(TEvent).Name, @event => when((TEvent)@event), (exception, @event) => onError(exception, (TEvent)@event)));
        }

        public void Handle(string eventType, object @event)
        {
            handlers
                .Where(handler => handler.EventType == eventType)
                .ToList()
                .ForEach(handler =>
                {
                    try
                    {
                        handler.Handler(@event);
                    }
                    catch (Exception ex)
                    {
                        handler.OnError(ex, @event);
                        throw;
                    }
                });
        }

        public bool CanHandle(string eventType)
        {
            return handlers.Any(h => h.EventType == eventType);
        }
    }
}