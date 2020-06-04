using System;
using System.Threading;
using System.Threading.Tasks;
using BankAccount.CoreDomain.Commands;
using MediatR;

namespace BankAccount.CoreDomain.CommandHandler
{
    public class WithdrawMoneyHandler : IRequestHandler<WithdrawMoney>
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public WithdrawMoneyHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<Unit> Handle(WithdrawMoney request, CancellationToken cancellationToken)
        {
            var bankAccount = await bankAccountRepository.GetByIdAsync(request.BankAccountId).ConfigureAwait(false);
            bankAccount.WithdrawMoney(request.Amount, request.Transaction, request.TimeStamp);
            await bankAccountRepository.SaveAsync(bankAccount).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}