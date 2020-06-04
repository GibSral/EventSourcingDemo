namespace BankAccount.CoreDomain.UnitTests.BankAccountTests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Special NamingConvention for tests")]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Special NamingConvention for tests")]
    public class WithdrawMoney
    {
        [Test]
        public void WithdrawMoney_WithLimitNotExceeded_WithdrawsMoney()
        {
        } 
    }
}