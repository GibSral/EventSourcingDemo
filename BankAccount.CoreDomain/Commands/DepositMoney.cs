using System;
using BankAccount.CoreDomain.DomainValues;
using MediatR;

namespace BankAccount.CoreDomain.Commands
{
    public class DepositMoney : Command, IRequest
    {
        public DepositMoney(OId<BankAccount, Guid> bankAccountId, Money amount, TimeStamp timeStamp)
            : base(timeStamp)
        {
            BankAccountId = bankAccountId;
            Amount = amount;
        }

        public OId<BankAccount, Guid> BankAccountId { get; }

        public Money Amount { get; }
    }
}