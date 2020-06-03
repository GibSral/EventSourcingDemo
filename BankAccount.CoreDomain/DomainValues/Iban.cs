using System;
using System.Text.RegularExpressions;
using static BankAccount.CoreDomain.Contracts;

namespace BankAccount.CoreDomain.DomainValues
{
    public sealed class Iban : IEquatable<Iban>
    {
        private static readonly Lazy<Regex> ValidationRegex = new Lazy<Regex>(() => new Regex(
            @"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant));

        private Iban(string value)
        {
            RequireParameter(value, () => nameof(value));
            Require(value, IsValidIban, it => $"Invalid IBAN - {it}");
            Value = value;
        }

        public string Value { get; }

        private static bool IsValidIban(string it) => ValidationRegex.Value.IsMatch(it);

        public static Iban Of(string iban) => new Iban(iban.Replace(" ", string.Empty).Trim());

        public bool Equals(Iban? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Value == other.Value;
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Iban other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(Iban? left, Iban? right) => Equals(left, right);

        public static bool operator !=(Iban? left, Iban? right) => !Equals(left, right);
    }
}