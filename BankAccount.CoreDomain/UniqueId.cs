using System;

namespace BankAccount.CoreDomain
{
    public static class UniqueId
    {
       private static Guid? GuidOverride { get; set; }

       public static Guid New() => GuidOverride ?? Guid.NewGuid();

       public static void OverrideNewGuid(Guid guid) => GuidOverride = guid;

       public static void ResetOverride() => GuidOverride = null;
    }
}