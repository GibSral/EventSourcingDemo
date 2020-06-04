using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain.UnitTests
{
    public class BankAccountBuilder
    {
        private readonly OId<BankAccount, Guid> id;
        private readonly OId<Employee, Guid> employeeId;
        private readonly OId<AccountHolder, Guid> accountHolderId;
        private readonly Iban iban;
        private readonly List<BankAccountEvent> events = new List<BankAccountEvent>();
        private Currency accountCurrency;

        public BankAccountBuilder(OId<BankAccount, Guid> id)
        {
            this.id = id;
            employeeId = OId.Of<Employee, Guid>(Guid.Parse("309dc64d-bde5-4ee5-9e21-33a517e2fe35"));
            accountHolderId = OId.Of<AccountHolder, Guid>(Guid.Parse("59ed2782-881b-49a9-8230-d0b3bb1c9072"));
            iban = Iban.Of("DE37200505501340426749");
            accountCurrency = Currency.Euro;
        }

        public BankAccountBuilder WithCurrency(Currency currency) => BuildStep(() => accountCurrency = currency);

        public BankAccountBuilder WithDepositedMoney(decimal amount) => BuildStep(() => events.Add(new MoneyDeposited(id.Value, Guid.Parse("c80cd533-0e27-4831-a8a4-15aaaeed983a"), amount, events.Count + 1)));

        public BankAccount Build()
        {
            var bankAccountCreated = new BankAccountCreated(id.Value, accountHolderId.Value, iban.Value, accountCurrency.Value, employeeId.Value, 1);
            events.Insert(0, bankAccountCreated);

            return BankAccount.Rehydrate(id, events);
        }

        private BankAccountBuilder BuildStep(Action buildStep)
        {
            buildStep();
            return this;
        }
    }
}