namespace ShiftManagement.Domain
{
    using System;
    using System.Collections.Generic;

    public class ContractType
    {
        public int Id { get; set; }

        public string ContractTypeName { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
