using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class DepositMoneyInAccount
    {
        [TestCase(100, "€")]
        [TestCase(200, "$")]
        public void DepositMoney_WithCorrectCurrency_DepositsMoneyInAccount(decimal amount, string currency)
        {
            var bankAccountRepository = Substitute.For<IBankAccountRepository>();
            var systemUnderTest = new DepositMoneyHandler(bankAccountRepository);
            var transaction = Transaction.Of(Guid.Parse("f0ad5ace-c64f-4f82-9487-a26b376dcf8b"));
            var accountCurrency = Currency.Of(currency);
            var command = new DepositMoney(DefaultBankAccountId, transaction, new Money(amount, accountCurrency), TimeStamp.Of(1));
            var bankAccount = RehydrateWithCreatedEvent(DefaultBankAccountId, accountCurrency);
            bankAccountRepository.GetByIdAsync(DefaultBankAccountId).Returns(Task.FromResult(bankAccount));

            systemUnderTest.Handle(command, CancellationToken.None).GetAwaiter().GetResult();

            var expectedEvents = new BankAccountEvent[] { new MoneyDeposited(DefaultBankAccountId.Value, transaction.Value, amount, 1) };
            bankAccountRepository.Received()
                .SaveAsync(Arg.Is<BankAccount>(it => it.IsCorrectBankAccount(aggregate =>
                    aggregate.Version == 2 &&
                    aggregate.InitialVersion == 1 &&
                    aggregate.GetUncommittedEvents().SequenceEqual(expectedEvents))));
        }

        [TestCase(100, "$", "€")]
        [TestCase(200, "€", "$")]
        public void DepositMoney_WithCurrencyMismatch_Throws(decimal amount, string currency, string accountCurrency)
        {
            var transaction = Transaction.Of(Guid.Parse("f0ad5ace-c64f-4f82-9487-a26b376dcf8b"));
            var bankAccountRepository = Substitute.For<IBankAccountRepository>();
            var systemUnderTest = new DepositMoneyHandler(bankAccountRepository);
            var bankAccount = RehydrateWithCreatedEvent(DefaultBankAccountId, Currency.Of(accountCurrency));
            var command = new DepositMoney(DefaultBankAccountId, transaction, new Money(amount, Currency.Of(currency)), TimeStamp.Of(1));
            bankAccountRepository.GetByIdAsync(DefaultBankAccountId).Returns(Task.FromResult(bankAccount));

            systemUnderTest.Awaiting(it => systemUnderTest.Handle(command, CancellationToken.None)).Should().Throw<CurrencyMismatchException>();
        }

        [Test]
        public void DepositMoney_WithBankAccountNotCreated_Throws()
        {
            var bankAccountRepository = Substitute.For<IBankAccountRepository>();
            var systemUnderTest = new DepositMoneyHandler(bankAccountRepository);
            var transaction = Transaction.Of(Guid.Parse("f0ad5ace-c64f-4f82-9487-a26b376dcf8b"));
            var command = new DepositMoney(DefaultBankAccountId, transaction, new Money(100, Currency.Euro), TimeStamp.Of(1));
            var bankAccount = BankAccount.Rehydrate(DefaultBankAccountId, Enumerable.Empty<BankAccountEvent>());
            bankAccountRepository.GetByIdAsync(DefaultBankAccountId).Returns(Task.FromResult(bankAccount));

            systemUnderTest.Awaiting(it => systemUnderTest.Handle(command, CancellationToken.None)).Should().Throw<ConstraintViolationException>();
        }
    }
}