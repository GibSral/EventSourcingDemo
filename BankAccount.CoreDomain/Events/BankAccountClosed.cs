using System;

namespace BankAccount.CoreDomain.Events
{
    public class BankAccountClosed : BankAccountEvent
    {
        public BankAccountClosed(Guid bankAccountId, long unixTimestamp)
            : base(bankAccountId, unixTimestamp)
        {
        }
    }
}