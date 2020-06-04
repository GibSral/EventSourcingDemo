using System;

namespace BankAccount.CoreDomain.DomainValues
{
    public sealed class Transaction : DomainValue<Guid>, IEquatable<Transaction>
    {
        private Transaction(Guid value)
            : base(value)
        {
        }

        public static Transaction Of(Guid transaction) => new Transaction(transaction);

        public bool Equals(Transaction? other) => base.Equals(other);

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Transaction other && Equals(other);

        public override int GetHashCode() => base.GetHashCode();
    }
}