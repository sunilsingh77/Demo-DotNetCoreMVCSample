using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo_DotNetCoreMVCSample.DTOs;

namespace Demo_DotNetCoreMVCSample.Repository
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        IEnumerable<Employee> GetAllEmployee();
        Employee Add(Employee employee);
    }
}
