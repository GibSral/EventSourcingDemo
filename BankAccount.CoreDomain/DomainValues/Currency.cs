using System;

namespace BankAccount.CoreDomain.DomainValues
{
    public class Currency : DomainValue<string>, IEquatable<Currency>
    {
        private Currency(string value)
            : base(value)
        {
            Contracts.RequireParameter(value, () => nameof(value));
        }

        public static Currency Of(string currency) => new Currency(currency);

        public static Currency Euro => new Currency("Euro");

        public static Currency Dollar => new Currency("Dollar");

        public bool Equals(Currency? other) => base.Equals(other);

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

            return Equals((Currency)obj);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}