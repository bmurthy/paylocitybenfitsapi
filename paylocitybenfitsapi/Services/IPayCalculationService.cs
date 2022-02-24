using paylocitybenfitsapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Services
{
    public interface IPayCalculationService
    {
        EmployeeCostToCompany Calculate(Employee employee);
    }
}
