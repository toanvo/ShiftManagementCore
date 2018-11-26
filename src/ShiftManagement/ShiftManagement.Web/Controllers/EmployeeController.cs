namespace ShiftManagement.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ShiftManagement.Domain;
    using ShiftManagement.Services.Interfaces;
    using ShiftManagement.Web.Filters;
    using ShiftManagement.Web.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    [Route("api/employee")]
    [ModelValidation]
    public class EmployeeController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper autoMapper, ILogger<EmployeeController> logger)
        {
            _mapper = autoMapper;
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await _employeeService.GetAllEmployee();
            return Ok(_mapper.Map<IEnumerable<EmployeeModel>>(employees));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            return Ok(_mapper.Map<EmployeeModel>(employee));
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody]EmployeeModel employeeModel)
        {
            if (employeeModel == null) return BadRequest();

            try
            {
                var currentEmployee = await _employeeService.GetEmployeeById(id);

                if (currentEmployee == null) return BadRequest($"The employee with id {id} does not exist!");

                _mapper.Map(employeeModel, currentEmployee);
                await _employeeService.UpdateEmployee(currentEmployee);
                return Ok(currentEmployee);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Updating employee has been raised an exception {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmployeeModel employeeModel)
        {
            if (employeeModel == null) return BadRequest();

            try
            {
                var newEmployee = _mapper.Map<Employee>(employeeModel);
                await _employeeService.CreateEmployee(newEmployee);
                employeeModel.Id = newEmployee.Id;

                return CreatedAtRoute(
                    routeName: "employee", 
                    routeValues: new { id = employeeModel.Id }, 
                    value: employeeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Saving the employee has been raised an exception {ex}");
                return BadRequest();
            }
        }


        [HttpGet("{{employeeIds}}", Name="GetEmployees")]
        public async Task<IActionResult> GetEmployees(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> employeeIds)
        {
            var employees = await _employeeService.GetEmployeeByIds(employeeIds);
            if (employeeIds.Count() != employees.Count)
            {
                return NotFound();
            }

            var result = _mapper.Map<IEnumerable<EmployeeModel>>(employees);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployees([FromBody]IEnumerable<EmployeeModel> employeeModel)
        {
            if (employeeModel == null) return BadRequest();

            try
            {
                var newEmployees = _mapper.Map<IEnumerable<Employee>>(employeeModel);
                foreach(var newEmployee in newEmployees)
                {
                    await _employeeService.CreateEmployee(newEmployee);
                }

                var employeeListResult = await _employeeService.GetEmployeeByIds(newEmployees.Select(x => x.Id));
                var employeeIdsResult = string.Join(",", employeeListResult.Select(x => x.Id));

                return CreatedAtRoute("GetEmployees", new { employeeIdsResult }, employeeListResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Saving the employee has been raised an exception {ex}");
                return BadRequest();
            }
        }
    }
}
