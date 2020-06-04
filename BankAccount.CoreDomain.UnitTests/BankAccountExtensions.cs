using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain.UnitTests
{
    public static class BankAccountExtensions
    {
        public static bool IsCorrectBankAccount(this BankAccount bankAccount, OId<BankAccount, Guid> accountId, int expectedVersion, params BankAccountEvent[] expectedEvents)
        {
            return bankAccount.IsCorrectBankAccount(accountId, expectedVersion, it => it.GetUncommittedEvents().SequenceEqual(expectedEvents));
        }
        
        public static bool IsCorrectBankAccount(this BankAccount bankAccount, OId<BankAccount, Guid> accountId, int expectedVersion, IEnumerable<BankAccountEvent> expectedEvents)
        {
            return bankAccount.IsCorrectBankAccount(accountId, expectedVersion, it => it.GetUncommittedEvents().SequenceEqual(expectedEvents));
        }

        public static bool IsCorrectBankAccount(this BankAccount bankAccount, OId<BankAccount, Guid> accountId, int expectedVersion, Func<IAggregateRoot<BankAccount, BankAccountEvent>, bool> assertion)
        {
            return bankAccount.IsCorrectBankAccount(it => it.Id.Equals(accountId) && it.Version.Equals(expectedVersion) && assertion(bankAccount));
        }

        public static bool IsCorrectBankAccount(this BankAccount bankAccount, Func<IAggregateRoot<BankAccount, BankAccountEvent>, bool> assertion) => assertion(bankAccount);
    }
}