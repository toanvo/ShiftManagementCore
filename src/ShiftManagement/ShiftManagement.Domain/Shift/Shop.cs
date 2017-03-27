namespace ShiftManagement.Domain
{
    using System;
    using System.Collections.Generic;

    public class Shop : IIdentity, IAudit
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }
        
        public Employee Manager { get; set; }

        public ICollection<Shift> Shifts { get; set; }

        public ICollection<ShopOpenHour> OpenHours { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
