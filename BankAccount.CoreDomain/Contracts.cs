using System;

namespace BankAccount.CoreDomain
{
    public static class Contracts
    {
        public static void RequireParameter(string str, Func<string> parameterName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ConstraintViolationException($"String must contain at least one character - VariableName: {parameterName()}");
            }
        }

        public static void RequireParameter(object? obj, Func<string> parameterName) => Require(obj, () => $"Variable must not be null - VariableName: {parameterName()}");

        public static void Require(object? obj, Func<string> errorMessage)
        {
            if (obj == null)
            {
                throw new ConstraintViolationException(errorMessage());
            }
        }

        public static void Require<TValue>(TValue value, Func<TValue, bool> validate, Func<TValue, string> errorMessage) where TValue : notnull
        {
            if (!validate(value))
            {
                throw new ConstraintViolationException(errorMessage(value));
            }
        }

        public static TReturn RequireTransformation<TValue, TReturn>(TValue value, Func<TValue, TReturn> transformation, Func<string> errorMessage)
        {
            try
            {
                return transformation(value);
            }
            catch (Exception ex)
            {
                throw new ConstraintViolationException(errorMessage(), ex);
            }
        }

        public static string Mandatory(string name) => $"{name} is mandatory";
    }
}