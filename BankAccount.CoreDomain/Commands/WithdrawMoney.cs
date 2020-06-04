﻿using System;
using BankAccount.CoreDomain.DomainValues;
using MediatR;

namespace BankAccount.CoreDomain.Commands
{
    public class WithdrawMoney : Command, IRequest
    {
        public WithdrawMoney(OId<BankAccount, Guid> bankAccountId, Transaction transaction, Money amount, TimeStamp timeStamp)
            : base(timeStamp)
        {
            BankAccountId = bankAccountId;
            Transaction = transaction;
            Amount = amount;
        }

        public OId<BankAccount, Guid> BankAccountId { get; }

        public Transaction Transaction { get; }

        public Money Amount { get; }
    }
}