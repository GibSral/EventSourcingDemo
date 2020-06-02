using System;

namespace BankAccount.CoreDomain.Events
{
    public sealed class DispoGranted : BankAccountEvent, IEquatable<DispoGranted>
    {
        public DispoGranted(Guid bankAccountId, long unixTimestamp, decimal amount, Guid grantedByEmployee)
            : base(bankAccountId, unixTimestamp)
        {
            Amount = amount;
            GrantedByEmployee = grantedByEmployee;
        }

        public decimal Amount { get; }

        public Guid GrantedByEmployee { get; }

        public bool Equals(DispoGranted? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Amount == other.Amount && GrantedByEmployee.Equals(other.GrantedByEmployee);
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is DispoGranted other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ GrantedByEmployee.GetHashCode();
                return hashCode;
            }
        }
    }
}