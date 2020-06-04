using System.Threading;
using System.Threading.Tasks;
using BankAccount.CoreDomain.Commands;
using MediatR;

namespace BankAccount.CoreDomain.CommandHandler
{
    public class DepositMoneyHandler : IRequestHandler<DepositMoney>
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public DepositMoneyHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<Unit> Handle(DepositMoney request, CancellationToken cancellationToken)
        {
            var bankAccount = await bankAccountRepository.GetByIdAsync(request.BankAccountId).ConfigureAwait(false);
            bankAccount.DepositMoney(request.Amount, request.Transaction, request.TimeStamp);
            await bankAccountRepository.SaveAsync(bankAccount).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}