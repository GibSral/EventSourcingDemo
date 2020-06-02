using System;

namespace BankAccount.CoreDomain.Entities
{
    public class Employee
    {
        public Employee(OId<Employee, Guid> id)
        {
            Id = id;
        }

        public OId<Employee, Guid> Id { get; }
    }
}