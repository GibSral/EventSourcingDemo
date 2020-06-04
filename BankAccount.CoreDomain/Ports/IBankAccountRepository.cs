using System;
using System.Threading.Tasks;

namespace BankAccount.CoreDomain
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetByIdAsync(OId<BankAccount, Guid> id);

        Task SaveAsync(BankAccount bankAccount);
    }
}