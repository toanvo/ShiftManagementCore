namespace ShiftManagement.Domain
{
    using System;
    using Microsoft.AspNetCore.Identity;

    public class EmployeeRole : IdentityRole<int>, IIdentity, IAudit
    {
        public string Description { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
