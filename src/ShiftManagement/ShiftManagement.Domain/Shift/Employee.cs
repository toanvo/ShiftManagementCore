namespace ShiftManagement.Domain
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class Employee : IdentityUser<int>, IIdentity
    {
        public Employee EmployeeManager { get; set; }

        public string Title { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public ContractType ContractType { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
