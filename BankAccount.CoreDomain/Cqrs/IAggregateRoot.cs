using System;
using System.Collections.Generic;

namespace BankAccount.CoreDomain.Cqrs
{
    public interface IAggregateRoot<TAggregate, TEvent> where TAggregate : notnull where TEvent : notnull
    {
        OId<TAggregate, Guid> Id { get; }

        int Version { get; }

        int InitialVersion { get; }

        IReadOnlyCollection<TEvent> GetUncommittedEvents();

        void OnEventsCommitted();

        void Replay(IEnumerable<TEvent> events);
    }
}