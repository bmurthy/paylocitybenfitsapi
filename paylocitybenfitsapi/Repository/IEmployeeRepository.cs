using paylocitybenfitsapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeCostToCompany>> GetEmployees();

        Task SaveEmployee(EmployeeCostToCompany employeeCostToCompany);
    }
}
