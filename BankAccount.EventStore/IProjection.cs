namespace BankAccount.EventStore
{
    public interface IProjection
    {
        string Id { get; }

        bool CanHandle(string eventType);

        void Handle(string eventType, object @event);
    }
}