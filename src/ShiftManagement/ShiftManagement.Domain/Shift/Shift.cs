namespace ShiftManagement.Domain
{
    using System;
    
    public class Shift : IIdentity, IAudit
    {
        public int Id { get; set; }
        
        public Employee Employee { get; set; }
        
        public Shop Shop { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Status Status { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
