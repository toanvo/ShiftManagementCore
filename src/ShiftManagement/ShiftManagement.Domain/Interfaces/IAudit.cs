using System;

namespace ShiftManagement.Domain
{
    public interface IAudit
    {
        int CreateUserId { get; set; }
        DateTime CreatedDate { get; set; }

        int UpdateUserId { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
