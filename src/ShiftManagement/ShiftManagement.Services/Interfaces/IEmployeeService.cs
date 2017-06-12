namespace ShiftManagement.Services.Interfaces
{
    using ShiftManagement.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployee();
        Task<Employee> GetEmployeeById(int employeeId);
    }
}
