using System;

namespace BankAccount.CoreDomain.Events
{
    public sealed class MoneyWithdrawn : MoneyTransferEvent, IEquatable<MoneyWithdrawn>
    {
        public MoneyWithdrawn(Guid bankAccountId, Guid transactionId, decimal amount, long unixTimestamp)
            : base(bankAccountId, transactionId, unixTimestamp)
        {
            Amount = amount;
        }

        public decimal Amount { get; }

        public bool Equals(MoneyWithdrawn? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Amount == other.Amount;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is MoneyWithdrawn other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Amount.GetHashCode();
            }
        }
    }
}