using System;
using BankAccount.CoreDomain.DomainValues;
using NSubstitute;

namespace BankAccount.CoreDomain.UnitTests
{
    public static class BankAccountTestAdapter
    {
        public static OId<BankAccount, Guid> DefaultBankAccountId { get; } = OId.Of<BankAccount, Guid>(Guid.Parse("5fd9c8fe-2a2c-4d76-9c57-2460dd516dbf"));

        public static BankAccount RehydrateWithCreatedEvent(OId<BankAccount, Guid> accountId) => RehydrateWithCreatedEvent(accountId, Currency.Euro);

        public static BankAccount RehydrateWithCreatedEvent(OId<BankAccount, Guid> accountId, Currency currency) =>
            new BankAccountBuilder(accountId).WithCurrency(currency).Build();

        public static IBankAccountRepository ConfigureBankAccountRepositoryForAccount(BankAccount bankAccount)
        {
            var bankAccountRepository = Substitute.For<IBankAccountRepository>();
            bankAccountRepository.GetByIdAsync(bankAccount.AggregateId).Returns(bankAccount);
            return bankAccountRepository;
        }
    }
}