using System;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Entities;
using MediatR;

namespace BankAccount.CoreDomain.Commands
{
    public class CreateBankAccount : Command, IRequest<OId<BankAccount, Guid>>
    {
        public CreateBankAccount(OId<AccountHolder, Guid> accountHolderId, OId<Employee, Guid> employeeId, Currency accountCurrency, Iban iban, TimeStamp timeStamp)
            : base(timeStamp)
        {
            AccountHolderId = accountHolderId;
            EmployeeId = employeeId;
            AccountCurrency = accountCurrency;
            Iban = iban;
        }

        public OId<AccountHolder, Guid> AccountHolderId { get; }

        public OId<Employee, Guid> EmployeeId { get; }

        public Currency AccountCurrency { get; }

        public Iban Iban { get; }
    }
}