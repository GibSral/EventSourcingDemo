using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static BankAccount.CoreDomain.Contracts;

namespace BankAccount.CoreDomain
{
    public sealed class OId<TEntity, TId> : IEquatable<OId<TEntity, TId>> where TEntity : notnull
    {
        private OId(TId value)
        {
            RequireParameter(value, () => nameof(value));
            Value = value;
        }

        public TId Value { get; }

        [SuppressMessage("Microsoft.Design", "CA1000:Do not declare static members on generic types", Justification = "type inference not possible because there is no object in the parameterlist that is Of type TEntity")]
        public static OId<TEntity, TId> Of(TId id) => new OId<TEntity, TId>(id);

        public bool Equals(OId<TEntity, TId>? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<TId>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is OId<TEntity, TId> other && Equals(other);

        public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Value);

        public static bool operator ==(OId<TEntity, TId>? left, OId<TEntity, TId>? right) => Equals(left, right);

        public static bool operator !=(OId<TEntity, TId>? left, OId<TEntity, TId>? right) => !Equals(left, right);
    }
}