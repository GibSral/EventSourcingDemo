using System;
using MediatR;

namespace BankAccount.CoreDomain.Commands
{
    public class CloseBankAccount : IRequest
    {
        public CloseBankAccount(OId<BankAccount, Guid> id)
        {
            Id = id;
        }

        public OId<BankAccount, Guid> Id { get; }
    }
}