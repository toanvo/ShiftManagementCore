namespace ShiftManagement.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShiftManagement.Domain;
    using ShiftManagement.Services.Interfaces;

    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> Get()
        {
            return await _service.GetAllEmployee();
        }

        [HttpGet("{id}")]
        public async Task<Employee> Get(int id)
        {
            return await _service.GetEmployeeById(id);
        }

        [HttpPost]
        public async Task Post([FromBody]Employee employee)
        {
            await _service.CreateEmployee(employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Employee employee)
        {
            var currentEmployee = await _service.GetEmployeeById(id);
            if (currentEmployee != null)
            {
                await _service.UpdateEmployee(employee);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var currentEmployee = await _service.GetEmployeeById(id);
            if (currentEmployee != null)
            {
                await _service.DeleteEmployee(id);
                return Ok();
            }

            return NotFound();
        }
    }
}
