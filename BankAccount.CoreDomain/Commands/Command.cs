using BankAccount.CoreDomain.DomainValues;

namespace BankAccount.CoreDomain.Commands
{
    public class Command
    {
        protected Command(TimeStamp timeStamp)
        {
            TimeStamp = timeStamp;
        }

        public TimeStamp TimeStamp { get; }
    }
}