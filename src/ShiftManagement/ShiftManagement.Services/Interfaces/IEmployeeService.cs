namespace ShiftManagement.Services.Interfaces
{
    using ShiftManagement.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployee();

        Task<Employee> GetEmployeeById(int employeeId);

        Task CreateEmployee(Employee employee);

        Task UpdateEmployee(Employee employee);

        Task DeleteEmployee(int employeeId);
    }
}
