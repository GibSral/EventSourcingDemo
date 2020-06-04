using System;

namespace BankAccount.CoreDomain.DomainValues
{
    public sealed class TimeStamp : DomainValue<long>, IEquatable<TimeStamp>
    {
        public TimeStamp(long value)
            : base(value)
        {
        }

        public bool Equals(TimeStamp? other) => base.Equals(other);

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is TimeStamp other && Equals(other);

        public override int GetHashCode() => base.GetHashCode();
    }
}