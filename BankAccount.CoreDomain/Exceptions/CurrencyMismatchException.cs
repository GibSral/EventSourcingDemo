using System;
using BankAccount.CoreDomain.DomainValues;

namespace BankAccount.CoreDomain.Exceptions
{
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(Currency expectedCurrency, Currency actualCurrency)
            : base($"Expected currency {expectedCurrency.Value} but got {actualCurrency.Value}")
        {
        }
    }
}