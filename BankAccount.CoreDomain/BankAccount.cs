using System;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain
{
    public class BankAccount : AggregateRoot<BankAccountEvent>, IApply<BankAccountCreated>, IApply<BankAccountClosed>, IApply<MoneyDeposited>, IApply<MoneyWithdrawn>, IApply<DispoGranted>, IApply<OwnerMoved>
    {
        public BankAccount(OId<BankAccount, Guid> id)
            : base(id)
        {
        }

        void IApply<BankAccountCreated>.Apply(BankAccountCreated @event)
        {
            throw new System.NotImplementedException();
        }

        void IApply<BankAccountClosed>.Apply(BankAccountClosed @event)
        {
            throw new System.NotImplementedException();
        }

        void IApply<MoneyDeposited>.Apply(MoneyDeposited @event)
        {
            throw new System.NotImplementedException();
        }

        void IApply<MoneyWithdrawn>.Apply(MoneyWithdrawn @event)
        {
            throw new System.NotImplementedException();
        }

        void IApply<DispoGranted>.Apply(DispoGranted @event)
        {
            throw new System.NotImplementedException();
        }

        void IApply<OwnerMoved>.Apply(OwnerMoved @event)
        {
            throw new System.NotImplementedException();
        }
    }
}