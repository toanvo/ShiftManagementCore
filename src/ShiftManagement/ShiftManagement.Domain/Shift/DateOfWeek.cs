namespace ShiftManagement.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DateOfWeek : IIdentity, IAudit
    {
        public int Id { get; set; }

        public ShopOpenHour ShopOpenHour { get; set; }

        public string Name { get; set; }
        
        public int CreateUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdateUserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
