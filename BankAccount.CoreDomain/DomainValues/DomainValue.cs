using System;
using System.Collections.Generic;

namespace BankAccount.CoreDomain.DomainValues
{
    public class DomainValue<T>
    {
        public DomainValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public bool Equals(DomainValue<T>? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(Value, other.Value);
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

            return Equals((DomainValue<T>)obj);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);
    }
}