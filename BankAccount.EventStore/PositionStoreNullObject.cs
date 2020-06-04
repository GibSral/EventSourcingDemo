using System;
using EventStore.ClientAPI;

namespace BankAccount.EventStore
{
    public class PositionStoreNullObject : IPositionStore
    {
        public Position GetLatest(string projection) => Position.Start;

        public void Update(string projection, Position? position)
        {
            // Nullobject implementation
        }

        public void OnError(string projection, string eventType, string streamId, Position? position, Exception exception)
        {
            // Nullobject implementation
        }
    }
}