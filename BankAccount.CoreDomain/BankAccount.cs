﻿using System;
using System.Collections.Generic;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using BankAccount.CoreDomain.Events;
using BankAccount.CoreDomain.Exceptions;

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
        private Currency? currency;
        private decimal balance;

        private BankAccount(OId<BankAccount, Guid> id)
            : base(id)
        {
        }

        public static BankAccount New(
            OId<BankAccount, Guid> id,
            OId<AccountHolder, Guid> accountHolderId,
            Iban iban,
            Currency accountCurrency,
            OId<Employee, Guid> employeeId,
            TimeStamp timeStamp)
        {
            var bankAccount = new BankAccount(id);
            bankAccount.Create(accountHolderId, iban, accountCurrency, employeeId, timeStamp);
            return bankAccount;
        }

        public static BankAccount Rehydrate(OId<BankAccount, Guid> id, IEnumerable<BankAccountEvent> events)
        {
            var bankAccount = new BankAccount(id);
            bankAccount.Replay(events);
            return bankAccount;
        }

        private void Create(
            OId<AccountHolder, Guid> accountHolderId,
            Iban accountIban,
            Currency accountCurrency,
            OId<Employee, Guid> employeeId,
            TimeStamp timeStamp)
        {
            RaiseEvent(new BankAccountCreated(AggregateId.Value,
                accountHolderId.Value,
                accountIban.Value,
                accountCurrency.Value,
                employeeId.Value,
                timeStamp.Value));
        }

        public void DepositMoney(Money amount, Transaction transaction, TimeStamp timeStamp)
        {
            RequiresCreatedAccount();
            RequiresCorrectCurrency(amount);
            RaiseEvent(new MoneyDeposited(AggregateId.Value, transaction.Value, amount.Amount, timeStamp.Value));
        }

        public void WithdrawMoney(Money amount, Transaction transaction, TimeStamp timeStamp)
        {
            RequiresCreatedAccount();
            RequiresCorrectCurrency(amount);
            RequiresEnoughMoney(amount);
            RaiseEvent(new MoneyWithdrawn(AggregateId.Value, transaction.Value, amount.Amount, timeStamp.Value));
        }

        private void RequiresEnoughMoney(Money amount)
        {
            if (amount.Amount > balance)
            {
                throw new LimitExceededException();
            }
        }

        void IApply<BankAccountCreated>.Apply(BankAccountCreated @event)
        {
            iban = Iban.Of(@event.Iban);
            currency = Currency.Of(@event.Currency);
        }

        void IApply<BankAccountClosed>.Apply(BankAccountClosed @event)
        {
            throw new NotImplementedException();
        }

        void IApply<MoneyDeposited>.Apply(MoneyDeposited @event)
        {
            balance += @event.Amount;
        }

        void IApply<MoneyWithdrawn>.Apply(MoneyWithdrawn @event)
        {
            balance -= @event.Amount;
        }

        void IApply<DispoGranted>.Apply(DispoGranted @event)
        {
            throw new NotImplementedException();
        }

        void IApply<GutschriftErhalten>.Apply(GutschriftErhalten @event)
        {
            throw new NotImplementedException();
        }

        private void RequiresCreatedAccount() => Contracts.Require(iban, () => "Account not created");

        private void RequiresCorrectCurrency(Money money)
        {
            if (!currency!.Equals(money.Currency))
            {
                throw new CurrencyMismatchException(currency, money.Currency);
            }
        }
    }
}