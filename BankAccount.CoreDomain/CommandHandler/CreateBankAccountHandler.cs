using System;
using System.Threading;
using System.Threading.Tasks;
using BankAccount.CoreDomain.Commands;
using MediatR;

namespace BankAccount.CoreDomain.CommandHandler
{
    public class CreateBankAccountHandler : IRequestHandler<CreateBankAccount, OId<BankAccount, Guid>>
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public CreateBankAccountHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<OId<BankAccount, Guid>> Handle(CreateBankAccount request, CancellationToken cancellationToken)
        {
            var bankAccountId = OId.Of<BankAccount, Guid>(UniqueId.New());
            var bankAccount = BankAccount.New(bankAccountId, request.AccountHolderId, request.Iban, request.AccountCurrency, request.EmployeeId, request.TimeStamp);
            await bankAccountRepository.SaveAsync(bankAccount).ConfigureAwait(false);
            return bankAccountId;
        }
    }
}