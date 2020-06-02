using System;

namespace BankAccount.CoreDomain.Events
{
    public class BankAccountCreated : BankAccountEvent
    {
        public BankAccountCreated(Guid bankAccountId, Guid clientId, string currency, long unixTimestamp)
            : base(bankAccountId, unixTimestamp)
        {
            ClientId = clientId;
            Currency = currency;
        }

        public Guid ClientId { get; }
        public string Currency { get; }
    }
}