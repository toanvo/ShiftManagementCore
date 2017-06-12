namespace ShiftManagement.Domain
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class Employee : IdentityUser<int>, IIdentity, IAudit
    {
        public Employee EmployeeManager { get; set; }

        public string Title { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public ContractType ContractType { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
