using System;

namespace BankAccount.CoreDomain.Events
{
    public class BankAccountEvent
    {
        protected BankAccountEvent(Guid bankAccountId, long unixTimestamp)
        {
            BankAccountId = bankAccountId;
            UnixTimestamp = unixTimestamp;
        }

        public Guid BankAccountId { get; }
        public long UnixTimestamp { get; }
    }
}