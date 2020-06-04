using System;
using BankAccount.CoreDomain.Cqrs;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain.UnitTests
{
    public static class BankAccountExtensions
    {
        public static bool IsCorrectBankAccount(this BankAccount bankAccount, Func<IAggregateRoot<BankAccount, BankAccountEvent>, bool> assertion) => assertion(bankAccount);
    }
}