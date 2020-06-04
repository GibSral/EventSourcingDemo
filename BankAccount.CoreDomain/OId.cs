using System;
using System.Collections.Generic;
using static BankAccount.CoreDomain.Contracts;

namespace BankAccount.CoreDomain
{
    public static class OId
    {
        public static OId<TEntity, TId> Of<TEntity, TId>(TId id) where TEntity : notnull => new OId<TEntity, TId>(id);
    }

    public sealed class OId<TEntity, TId> : IEquatable<OId<TEntity, TId>> where TEntity : notnull
    {
        public OId(TId value)
        {
            RequireParameter(value, () => nameof(value));
            Value = value;
        }

        public TId Value { get; }

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