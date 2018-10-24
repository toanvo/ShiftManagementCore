using Microsoft.AspNetCore.Hosting;

namespace ShiftManagement.Web.Data
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using ShiftManagement.DataAccess;
    using ShiftManagement.Domain;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class DataSeeder
    {   
        private ShiftManagementDbContext DbContext;
        private RoleManager<EmployeeRole> RoleManager;
        private UserManager<Employee> UserManager;

        public DataSeeder(ShiftManagementDbContext dbContext, IHostingEnvironment environment, RoleManager<EmployeeRole> roleManager, UserManager<Employee> userManager)
        {
            DbContext = dbContext;
            RoleManager = roleManager;
            UserManager = userManager;
        }

        public async Task SeedAsync()
        {
            DbContext.Database.EnsureCreated();
            if (DbContext.Users.Local.Count == 0)
                await CreateUsersAsync();
        }

        private async Task CreateUsersAsync()
        {
            // local variables    
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;
            string adminRole = "Administrators";
            string managerRole = "Manager";
            string employeeRole = "Registered";

            //Create Roles (if they doesn't exist yet)    
            if (!await RoleManager.RoleExistsAsync(adminRole))
            {
                await RoleManager.CreateAsync(new EmployeeRole() { Name = adminRole, CreatedDate = createdDate, UpdatedDate = lastModifiedDate, CreateUserId = 0, UpdateUserId = 0 });
            }

            if (!await RoleManager.RoleExistsAsync(employeeRole))
            {
                await RoleManager.CreateAsync(new EmployeeRole() { Name = employeeRole, CreatedDate = createdDate, UpdatedDate = lastModifiedDate, CreateUserId = 0, UpdateUserId = 0 });
            }

            if (!await RoleManager.RoleExistsAsync(managerRole))
            {
                await RoleManager.CreateAsync(new EmployeeRole() { Name = managerRole, CreatedDate = createdDate, UpdatedDate = lastModifiedDate, CreateUserId = 0, UpdateUserId = 0 });
            }

            // Create the "Admin" ApplicationUser account (if it doesn't exist already)    
            var adminUser = new Employee()
            {
                UserName = "Admin",
                Email = "admin@shift.com",
                CreatedDate = createdDate,
                UpdatedDate = lastModifiedDate,
                CreateUserId = 0, // has been generated automatically
                UpdateUserId = 0, // has been generated automatically
            };

            await SaveEmployeeAsync(adminUser, "Pass4admin", adminRole);

#if DEBUG
            var johnDoeUser = new Employee()
            {
                UserName = "johndoe",
                Email = "johndoe@shift.com",
                CreatedDate = createdDate,
                CreateUserId = 0, // has been generated automatically
                UpdateUserId = 0, // has been generated automatically
            };

            await SaveEmployeeAsync(johnDoeUser, "Pass4john", employeeRole);

            var aliceUser = new Employee()
            {
                UserName = "Alice",
                Email = "alice@shift.com",
                CreatedDate = createdDate,
                UpdatedDate = lastModifiedDate,
                CreateUserId = 0, // has been generated automatically
                UpdateUserId = 0, // has been generated automatically
            };

            await SaveEmployeeAsync(aliceUser, "Pass4alice", employeeRole);

            var robertUser = new Employee()
            {
                UserName = "robert",
                Email = "robert@shift.com",
                CreatedDate = createdDate,
                UpdatedDate = lastModifiedDate,
                CreateUserId = 0, // has been generated automatically
                UpdateUserId = 0, // has been generated automatically
            };

            await SaveEmployeeAsync(robertUser, "Pass4robert", managerRole);
#endif
            await DbContext.SaveChangesAsync();
        }

        private async Task SaveEmployeeAsync(Employee employee, string password, string role)
        {
            if (employee == null)
            {
                return;
            }

            if (await UserManager.FindByNameAsync(employee.UserName) == null)
            {
                await UserManager.CreateAsync(employee, password);
                await UserManager.AddToRoleAsync(employee, role);

                // Remove Lockout and E-Mail confirmation.        
                employee.EmailConfirmed = true;
                employee.LockoutEnabled = false;
            }
        }
    }
}
