using System;
using System.Collections.Generic;
using BankAccount.CoreDomain;
using BankAccount.CoreDomain.DomainValues;
using BankAccount.CoreDomain.Events;
using BankAccount.EventStore;

namespace BankAccount.Console
{
    public class AvailableAccountsProjection : Projection
    {
        private Dictionary<string, Guid> accounts = new Dictionary<string, Guid>();

        public AvailableAccountsProjection()
        {
            When<BankAccountCreated>(it => accounts[it.Iban] = it.BankAccountId);
        }
        
        public override string Id { get; } = "AvailableAccounts";

        public Guid GetId(Iban iban) => accounts[iban.Value];
    }
}