using System;

namespace BankAccount.CoreDomain.DomainValues
{
    public sealed class TimeStamp : DomainValue<long>, IEquatable<TimeStamp>
    {
        private TimeStamp(long value)
            : base(value)
        {
        }

        public static TimeStamp Of(long unixTimestamp) => new TimeStamp(unixTimestamp);

        public bool Equals(TimeStamp? other) => base.Equals(other);

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is TimeStamp other && Equals(other);

        public override int GetHashCode() => base.GetHashCode();
    }
}