using Microsoft.EntityFrameworkCore.Storage;

namespace ShiftManagement.DataAccess
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using ShiftManagement.Domain;

    public class ShiftManagementDbContext : IdentityDbContext<Employee, EmployeeRole, int>
    {
        public ShiftManagementDbContext(DbContextOptions<ShiftManagementDbContext> options) : base(options)
        {   
        }
        
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<ShopOpenHour> ShopOpenHour { get; set; }
    }
}
