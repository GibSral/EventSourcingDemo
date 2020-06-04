using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using BankAccount.CoreDomain.CommandHandler;
using BankAccount.CoreDomain.Commands;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Events;
using BankAccount.CoreDomain.Exceptions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using static BankAccount.CoreDomain.UnitTests.BankAccountTestAdapter;

namespace BankAccount.CoreDomain.UnitTests.BankAccountTests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Special NamingConvention for tests")]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Special NamingConvention for tests")]
    public class WithdrawMoneyFromAccount
    {
        [Test]
        public void WithdrawMoney_WithLimitNotExceeded_WithdrawsMoney()
        {
            var bankAccount = new BankAccountBuilder(DefaultBankAccountId).WithCurrency(Currency.Euro).WithDepositedMoney(50).WithDepositedMoney(50).Build();
            var bankAccountRepository = ConfigureBankAccountRepositoryForAccount(bankAccount);
            var systemUnderTest = new WithdrawMoneyHandler(bankAccountRepository);
            var command = new WithdrawMoney(DefaultBankAccountId, Transaction.Of(Guid.Parse("996228f4-70cb-4e44-9716-8a2a3b27d18c")), new Money(100, Currency.Euro), TimeStamp.Of(3));

            systemUnderTest.Handle(command, CancellationToken.None).GetAwaiter().GetResult();

            var moneyDeposited = new MoneyWithdrawn(DefaultBankAccountId.Value, command.Transaction.Value, command.Amount.Amount, command.TimeStamp.Value);
            bankAccountRepository.Received().SaveAsync(Arg.Is<BankAccount>(it => it.IsCorrectBankAccount(DefaultBankAccountId, 4, moneyDeposited)));
        }

        [Test]
        public void WithdrawMoney_WithLimitExceeded_Throws()
        {
            var bankAccount = new BankAccountBuilder(DefaultBankAccountId).WithCurrency(Currency.Euro).WithDepositedMoney(100).Build();
            var bankAccountRepository = ConfigureBankAccountRepositoryForAccount(bankAccount);
            var systemUnderTest = new WithdrawMoneyHandler(bankAccountRepository);
            var command = new WithdrawMoney(DefaultBankAccountId, Transaction.Of(Guid.Parse("996228f4-70cb-4e44-9716-8a2a3b27d18c")), new Money(200, Currency.Euro), TimeStamp.Of(3));

            systemUnderTest.Awaiting(it => it.Handle(command, CancellationToken.None)).Should().Throw<LimitExceededException>();
        }
        
        [Test]
        public void WithdrawMoney_OnUninitializedBankAccount_Throws()
        {
            var bankAccount = BankAccount.Rehydrate(DefaultBankAccountId, Enumerable.Empty<BankAccountEvent>());
            var bankAccountRepository = ConfigureBankAccountRepositoryForAccount(bankAccount);
            var systemUnderTest = new WithdrawMoneyHandler(bankAccountRepository);
            var command = new WithdrawMoney(DefaultBankAccountId, Transaction.Of(Guid.Parse("996228f4-70cb-4e44-9716-8a2a3b27d18c")), new Money(200, Currency.Euro), TimeStamp.Of(3));

            systemUnderTest.Awaiting(it => it.Handle(command, CancellationToken.None)).Should().Throw<ConstraintViolationException>();
        }

        [Test]
        public void WithdrawMoney_WithCurrencyMismatch_Throws()
        {
            var bankAccount = new BankAccountBuilder(DefaultBankAccountId).WithCurrency(Currency.Euro).WithDepositedMoney(100).Build();
            var bankAccountRepository = ConfigureBankAccountRepositoryForAccount(bankAccount);
            var systemUnderTest = new WithdrawMoneyHandler(bankAccountRepository);
            var command = new WithdrawMoney(DefaultBankAccountId,
                Transaction.Of(Guid.Parse("996228f4-70cb-4e44-9716-8a2a3b27d18c")),
                new Money(100, Currency.Dollar),
                TimeStamp.Of(3));

            systemUnderTest.Awaiting(it => it.Handle(command, CancellationToken.None)).Should().Throw<CurrencyMismatchException>();
        }
    }
}