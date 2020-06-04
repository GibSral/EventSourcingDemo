using System;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using BankAccount.CoreDomain.Events;

namespace BankAccount.CoreDomain.UnitTests
{
    public static class BankAccountTestAdapter
    {
        public static OId<BankAccount, Guid> DefaultBankAccountId { get; } = OId.Of<BankAccount, Guid>(Guid.Parse("5fd9c8fe-2a2c-4d76-9c57-2460dd516dbf"));

        public static BankAccount RehydrateWithCreatedEvent(OId<BankAccount, Guid> accountId) => RehydrateWithCreatedEvent(accountId, Currency.Euro);

        public static BankAccount RehydrateWithCreatedEvent(OId<BankAccount, Guid> accountId, Currency currency)
        {
            var employeeId = OId.Of<Employee, Guid>(Guid.Parse("309dc64d-bde5-4ee5-9e21-33a517e2fe35"));
            var accountHolderId = OId.Of<AccountHolder, Guid>(Guid.Parse("59ed2782-881b-49a9-8230-d0b3bb1c9072"));
            var iban = Iban.Of("DE37200505501340426749");
            var bankAccountCreated = new BankAccountCreated(accountId.Value, accountHolderId.Value, iban.Value, currency.Value, employeeId.Value, 1);
            var creationEvents = new BankAccountEvent[] { bankAccountCreated };
            return BankAccount.Rehydrate(accountId, creationEvents);
        }
    }
}