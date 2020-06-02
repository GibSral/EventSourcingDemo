using System;

namespace BankAccount.CoreDomain.Events
{
    public class BankAccountEvent
    {
        protected BankAccountEvent(Guid bankAccountId, long unixTimestamp)
        {
            BankAccountId = bankAccountId;
            UnixTimestamp = unixTimestamp;
        }

        public Guid BankAccountId { get; }
        public long UnixTimestamp { get; }

        public bool Equals(BankAccountEvent? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return BankAccountId.Equals(other.BankAccountId) && UnixTimestamp == other.UnixTimestamp;
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

            return Equals((BankAccountEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (BankAccountId.GetHashCode() * 397) ^ UnixTimestamp.GetHashCode();
            }
        }
    }
}