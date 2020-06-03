namespace BankAccount.CoreDomain.Cqrs
{
    internal interface IApply<in TEvent>
    {
        void Apply(TEvent @event);
    }
}