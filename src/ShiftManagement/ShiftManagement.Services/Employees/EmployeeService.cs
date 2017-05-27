namespace ShiftManagement.Services.Employees
{
    using ShiftManagement.DataAccess.Interfaces;
    using ShiftManagement.Domain;
    using ShiftManagement.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Employee>> GetAllEmployee()
        {
            return _unitOfWork.GetRepository<Employee>().GetAllAsync();
        }

        public Task<Employee> GetEmployeeById(int employeeId)
        {
            return _unitOfWork.GetRepository<Employee>().GetByIdAsync(employeeId);
        }

        public Task CreateEmployee(Employee employee)
        {
            _unitOfWork.GetRepository<Employee>().Insert(employee);
            return _unitOfWork.CommitAsync();
        }

        public Task UpdateEmployee(Employee employee)
        {
            _unitOfWork.GetRepository<Employee>().Update(employee);
            return _unitOfWork.CommitAsync();
        }

        public Task DeleteEmployee(int employeeId)
        {
            _unitOfWork.GetRepository<Employee>().Delete(employeeId);
            return _unitOfWork.CommitAsync();
        }
    }
}
