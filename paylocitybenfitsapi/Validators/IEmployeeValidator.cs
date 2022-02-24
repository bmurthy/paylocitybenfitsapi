using paylocitybenfitsapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Validators
{
    public interface IEmployeeValidator
    {
        bool IsValidEmployee(Employee employee);
    }
}
