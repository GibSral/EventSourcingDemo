using System;
using EventStore.ClientAPI;

namespace BankAccount.EventStore
{
    public interface IPositionStore
    {
        Position GetLatest(string projection);

        void Update(string projection, Position? position);

        void OnError(string projection, string eventType, string streamId, Position? position, Exception exception);
    }
}