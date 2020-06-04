using System;
using System.Collections.Generic;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Events;
using BankAccount.EventStore;

namespace BankAccount.AllAccountsProjection
{
    public class ShowBalancesProjection : Projection
    {
        private readonly Dictionary<Guid, string> oidToIbans = new Dictionary<Guid, string>();
        private readonly Dictionary<Guid, string> currencies = new Dictionary<Guid, string>();
        private readonly Dictionary<Guid, decimal> balances = new Dictionary<Guid, decimal>();
        public ShowBalancesProjection()
        {
            When<BankAccountCreated>(it =>
            {
                oidToIbans.Add(it.BankAccountId, it.Iban);
                currencies.Add(it.BankAccountId, it.Currency);
                balances.Add(it.BankAccountId, 0);

                ShowBalances();
            });

            When<MoneyDeposited>(it =>
            {
                var balance = balances[it.BankAccountId];
                balances[it.BankAccountId] = balance + it.Amount;
                ShowBalances();
            });
            
            When<MoneyWithdrawn>(it =>
            {
                var balance = balances[it.BankAccountId];
                balances[it.BankAccountId] = balance - it.Amount;
                ShowBalances();
            });
        }

        public override string Id { get; } = "AccountBalances";

        private void ShowBalances()
        {
            Console.Clear();
            foreach (var account in oidToIbans)
            {
                Console.WriteLine($"{account.Value} {balances[account.Key]} {currencies[account.Key]}");
            }
        }
    }
}