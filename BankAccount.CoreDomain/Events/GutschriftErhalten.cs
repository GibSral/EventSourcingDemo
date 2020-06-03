using System;

namespace BankAccount.CoreDomain.Events
{
    public sealed class GutschriftErhalten : BankAccountEvent, IEquatable<GutschriftErhalten>
    {
        public GutschriftErhalten(Guid bankAccountId, long unixTimestamp, decimal amount, string reason)
            : base(bankAccountId, unixTimestamp)
        {
            Amount = amount;
            Reason = reason;
        }

        public decimal Amount { get; }

        public string Reason { get; }

        public bool Equals(GutschriftErhalten? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Amount == other.Amount && Reason == other.Reason;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((GutschriftErhalten)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ Reason.GetHashCode();
                return hashCode;
            }
        }
    }
}