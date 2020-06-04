using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankAccount.CoreDomain.CommandHandler;
using BankAccount.CoreDomain.Commands;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using BankAccount.CoreDomain.Events;
using NSubstitute;

namespace BankAccount.CoreDomain.UnitTests.BankAccountTests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Special NamingConvention for tests")]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Special NamingConvention for tests")]
    public class CreateNewBankAccount
    {
        [Test]
        public void CreateNewBankAccount_Always_SavesNewBankAccount()
        {
            var bankAccountRepository = Substitute.For<IBankAccountRepository>();
            var systemUnderTest = new CreateBankAccountHandler(bankAccountRepository);
            var employeeId = OId.Of<Employee, Guid>(Guid.Parse("309dc64d-bde5-4ee5-9e21-33a517e2fe35"));
            var accountHolderId = OId.Of<AccountHolder, Guid>(Guid.Parse("59ed2782-881b-49a9-8230-d0b3bb1c9072"));
            var iban = Iban.Of("DE37200505501340426749");
            const int unixTimestamp = 1;
            var command = new CreateBankAccount(accountHolderId, employeeId, Currency.Euro, iban, new TimeStamp(unixTimestamp));
            var expectedAccountId = Guid.Parse("5fd9c8fe-2a2c-4d76-9c57-2460dd516dbf");
            UniqueId.OverrideNewGuid(expectedAccountId);

            systemUnderTest.Handle(command, CancellationToken.None).GetAwaiter().GetResult();

            var expectedEvents = new BankAccountEvent[] { new BankAccountCreated(expectedAccountId, accountHolderId.Value, iban.Value, Currency.Euro.Value, employeeId.Value, unixTimestamp) };
            bankAccountRepository.Received()
                .SaveAsync(Arg.Is<CoreDomain.BankAccount>(it => it.IsCorrectBankAccount(aggregate =>
                    aggregate.Id.Value.Equals(expectedAccountId) &&
                    aggregate.Version == 1 &&
                    aggregate.GetUncommittedEvents().SequenceEqual(expectedEvents))));
            UniqueId.ResetOverride();
        }
    }
}