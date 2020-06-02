using System;

namespace BankAccount.CoreDomain.Events
{
    public sealed class BankAccountCreated : BankAccountEvent, IEquatable<BankAccountCreated>
    {
        public BankAccountCreated(Guid bankAccountId, long unixTimestamp, Guid clientId, string currency, Guid createdByEmployee)
            : base(bankAccountId, unixTimestamp)
        {
            ClientId = clientId;
            Currency = currency;
            CreatedByEmployee = createdByEmployee;
        }

        public Guid ClientId { get; }

        public string Currency { get; }

        public Guid CreatedByEmployee { get; }

        public bool Equals(BankAccountCreated? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && ClientId.Equals(other.ClientId) && Currency == other.Currency && CreatedByEmployee.Equals(other.CreatedByEmployee);
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is BankAccountCreated other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ ClientId.GetHashCode();
                hashCode = (hashCode * 397) ^ Currency.GetHashCode();
                hashCode = (hashCode * 397) ^ CreatedByEmployee.GetHashCode();
                return hashCode;
            }
        }
    }
}