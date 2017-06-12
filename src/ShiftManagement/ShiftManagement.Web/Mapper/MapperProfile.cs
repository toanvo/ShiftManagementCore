using AutoMapper;
using ShiftManagement.Domain;
using ShiftManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftManagement.Web.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<Employee, EmployeeModel>().ReverseMap();
        }
    }
}
