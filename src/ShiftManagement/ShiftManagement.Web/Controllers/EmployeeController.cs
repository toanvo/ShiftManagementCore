using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShiftManagement.Services.Interfaces;
using ShiftManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftManagement.Web.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, IMapper autoMapper)
        {
            _mapper = autoMapper;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeService.GetAllEmployee();
            return Ok(_mapper.Map<IEnumerable<EmployeeModel>>(result));
        }
    }
}
