namespace BankAccount.CoreDomain.Cqrs
{
    public interface IApply<in TEvent>
    {
        void Apply(TEvent @event);
    }
}