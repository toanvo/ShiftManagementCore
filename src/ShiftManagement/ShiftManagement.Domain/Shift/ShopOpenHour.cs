namespace ShiftManagement.Domain
{
    using System;
    using System.Collections.Generic;

    public class ShopOpenHour : IIdentity, IAudit
    {
        public int Id { get; set; }

        public DateTime OpenedTime { get; set; }

        public DateTime ClosedTime { get; set; }

        public Shop Shop { get; set; }

        public ICollection<DateOfWeek> AvailableDateOfWeek { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
