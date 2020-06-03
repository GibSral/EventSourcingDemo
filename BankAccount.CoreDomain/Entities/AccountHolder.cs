using System;

namespace BankAccount.CoreDomain.Entities
{
    public class AccountHolder
    {
        public AccountHolder(OId<AccountHolder, Guid> id)
        {
            Id = id;
        }

        public OId<AccountHolder, Guid> Id { get; }
    }
}