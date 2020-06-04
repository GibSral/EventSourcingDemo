using System;
using System.Collections;
using System.Collections.Generic;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain
{
    public class BankAccount : AggregateRoot<BankAccount, BankAccountEvent>,
        IApply<BankAccountCreated>,
        IApply<BankAccountClosed>,
        IApply<MoneyDeposited>,
        IApply<MoneyWithdrawn>,
        IApply<DispoGranted>,
        IApply<GutschriftErhalten>
    {
        private Iban? iban;

        private BankAccount(OId<BankAccount, Guid> id)
            : base(id)
        {
        }

        public static BankAccount New(OId<BankAccount, Guid> id, OId<AccountHolder, Guid> accountHolderId, Iban iban, Currency accountCurrency, OId<Employee, Guid> employeeId, TimeStamp timeStamp)
        {
            var bankAccount = new BankAccount(id);
            bankAccount.Create(accountHolderId, iban, accountCurrency, employeeId, timeStamp);
            return bankAccount;
        }

        public static BankAccount Rehydrate(OId<BankAccount, Guid> id, IEnumerable<BankAccountEvent> events)
        {
            throw new NotImplementedException();
        }

        private void Create(
            OId<AccountHolder, Guid> accountHolderId,
            Iban accountIban,
            Currency accountCurrency,
            OId<Employee, Guid> employeeId,
            TimeStamp timeStamp)
        {
            RaiseEvent(new BankAccountCreated(AggregateId.Value, accountHolderId.Value, accountIban.Value, accountCurrency.Value, employeeId.Value, timeStamp.Value));
        }

        void IApply<BankAccountCreated>.Apply(BankAccountCreated @event)
        {
            iban = Iban.Of(@event.Iban);
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