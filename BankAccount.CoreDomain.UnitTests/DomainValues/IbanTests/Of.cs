using System.Diagnostics.CodeAnalysis;
using BankAccount.CoreDomain.DomainValues;
using FluentAssertions;
using NUnit.Framework;

namespace BankAccount.CoreDomain.UnitTests.DomainValues.IbanTests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Special NamingConvention for tests")]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Special NamingConvention for tests")]
    public class Of
    {
        [TestCase("DE37200505501340426749", "DE37200505501340426749")]
        [TestCase("  DE37200505501340426749", "DE37200505501340426749")]
        [TestCase("DE37200505501340426749  ", "DE37200505501340426749")]
        [TestCase("    DE37200505501340426749  ", "DE37200505501340426749")]
        [TestCase("    DE 3720 0505  5013 4042 67 49  ", "DE37200505501340426749")]
        public void Build_WithValidIban_ReturnsIban(string validIban, string expectedIban)
        {
            var systemUnderTest = Iban.Of(validIban);

            systemUnderTest.Value.Should().Be(expectedIban);
        }

        [Test]
        public void Build_WithInvalidIban_ThrowsException()
        {
            const string invalidIban = "invalidIban";

            this.Invoking(_ => Iban.Of(invalidIban)).Should().Throw<ConstraintViolationException>();
        }
    }
}