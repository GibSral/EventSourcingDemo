using System;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain
{
    public class BankAccount : AggregateRoot<BankAccountEvent>,
        IApply<BankAccountCreated>,
        IApply<BankAccountClosed>,
        IApply<MoneyDeposited>,
        IApply<MoneyWithdrawn>,
        IApply<DispoGranted>,
        IApply<GutschriftErhalten>
    {
        public BankAccount(OId<BankAccount, Guid> id, Iban iban)
            : base(id)
        {
            Iban = iban;
        }

        public Iban Iban { get; }

        void IApply<BankAccountCreated>.Apply(BankAccountCreated @event)
        {
            throw new NotImplementedException();
        }

        void IApply<BankAccountClosed>.Apply(BankAccountClosed @event)
        {
            throw new NotImplementedException();
        }

        void IApply<MoneyDeposited>.Apply(MoneyDeposited @event)
        {
            throw new NotImplementedException();
        }

        void IApply<MoneyWithdrawn>.Apply(MoneyWithdrawn @event)
        {
            throw new NotImplementedException();
        }

        void IApply<DispoGranted>.Apply(DispoGranted @event)
        {
            throw new NotImplementedException();
        }

        void IApply<GutschriftErhalten>.Apply(GutschriftErhalten @event)
        {
            throw new NotImplementedException();
        }
    }
}