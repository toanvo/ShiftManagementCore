namespace ShiftManagement.Services.Employees
{
    using ShiftManagement.DataAccess.Interfaces;
    using ShiftManagement.Domain;
    using ShiftManagement.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            return await _unitOfWork.GetRepository<Employee>().GetAllAsync();
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            return await _unitOfWork.GetRepository<Employee>().GetByIdAsync(employeeId);
        }

        public async Task CreateEmployee(Employee employee)
        {
            _unitOfWork.GetRepository<Employee>().Insert(employee);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _unitOfWork.GetRepository<Employee>().Update(employee);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteEmployee(int employeeId)
        {
            _unitOfWork.GetRepository<Employee>().Delete(employeeId);
            await _unitOfWork.CommitAsync();
        }
    }
}
