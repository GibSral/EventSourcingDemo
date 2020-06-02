using System;
using System.Collections.Generic;

namespace BankAccount.CoreDomain.Cqrs
{
    public interface IAggregateRoot<TEvent> where TEvent : notnull
    {
        OId<BankAccount, Guid> Id { get; }
        int Version { get; }
        int InitialVersion { get; }
        IReadOnlyCollection<TEvent> GetUncommittedEvents();
        void OnEventsCommitted();
        void Replay(IEnumerable<TEvent> events);
    }
}