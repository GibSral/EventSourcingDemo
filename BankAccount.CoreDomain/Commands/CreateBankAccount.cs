using System;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using MediatR;

namespace BankAccount.CoreDomain.Commands
{
    public class CreateBankAccount : Command, IRequest<OId<BankAccount, Guid>>
    {
        public CreateBankAccount(OId<AccountHolder, Guid> accountHolderId, OId<Employee, Guid> employeeId, TimeStamp timeStamp)
            : base(timeStamp)
        {
            AccountHolderId = accountHolderId;
            EmployeeId = employeeId;
        }

        public OId<AccountHolder, Guid> AccountHolderId { get; }

        public OId<Employee, Guid> EmployeeId { get; }
    }
}