using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAccount.CoreDomain;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace BankAccount.EventStore
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly Func<Task<IEventStoreConnection>> connect;

        public BankAccountRepository(Func<Task<IEventStoreConnection>> connect)
        {
            this.connect = connect;
        }

        public async Task<CoreDomain.BankAccount> GetByIdAsync(OId<CoreDomain.BankAccount, Guid> id)
        {
            var eventStoreConnection = await connect().ConfigureAwait(false);
            var events = await eventStoreConnection.ReadStreamEventsForwardAsync(MakeStreamName(nameof(CoreDomain.BankAccount), id.Value), 0, 4000, false);
            var bankAccountEvents = events.Events.Select(it => Encoding.UTF8.GetString(it.Event.Data)).Select(it => JsonConvert.DeserializeObject(it, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects })).Cast<BankAccountEvent>();
            return CoreDomain.BankAccount.Rehydrate(id, bankAccountEvents);
        }

        public async Task SaveAsync(CoreDomain.BankAccount bankAccount)
        {
            var aggregateRoot = (IAggregateRoot<CoreDomain.BankAccount, BankAccountEvent>)bankAccount;
            var events = aggregateRoot.GetUncommittedEvents()
                .Select(it => (@event: it, serializedEvent: JsonConvert.SerializeObject(it, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects })))
                .Select(it => (it.@event, data: Encoding.UTF8.GetBytes(it.serializedEvent)))
                .Select(it => new EventData(Guid.NewGuid(), it.@event.GetType().Name, true, it.data, new byte[0]));
            var eventStoreConnection = await connect().ConfigureAwait(false);
            await eventStoreConnection.AppendToStreamAsync(MakeStreamName(nameof(CoreDomain.BankAccount), aggregateRoot.Id.Value), aggregateRoot.InitialVersion, events)
                .ConfigureAwait(false);
        }

        private static string MakeStreamName(string aggregateName, Guid aggregateId) => $"{aggregateName}-{aggregateId:N}";
    }
}