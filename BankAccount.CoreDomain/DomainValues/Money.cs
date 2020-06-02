namespace BankAccount.CoreDomain.DomainValues
{
    public class Money 
    {
        public Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }

        public Currency Currency { get; }
    }
}