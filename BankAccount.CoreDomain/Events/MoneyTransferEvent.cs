using System;

namespace BankAccount.CoreDomain.Events
{
    public class MoneyTransferEvent : BankAccountEvent
    {
        protected MoneyTransferEvent(Guid bankAccountId, Guid transactionId, long unixTimestamp)
            : base(bankAccountId, unixTimestamp)
        {
            TransactionId = transactionId;
        }

        public Guid TransactionId { get; }

        public bool Equals(MoneyTransferEvent? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && TransactionId.Equals(other.TransactionId);
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

            return Equals((MoneyTransferEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ TransactionId.GetHashCode();
            }
        }
    }
}