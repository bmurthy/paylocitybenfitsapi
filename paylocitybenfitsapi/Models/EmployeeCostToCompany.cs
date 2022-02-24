using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Models
{
    public  class EmployeeCostToCompany
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DependentName1 { get; set; }

        public string DependentName2 { get; set; }

        public string DependentName3 { get; set; }

        public string DependentName4 { get; set; }
        public string Relation1 { get; set; }

        public string Relation2 { get; set; }

        public string Relation3 { get; set; }

        public string Relation4 { get; set; }

        public double Salary { get; set; }

        public double Benefits { get; set; }

        public double CostToCompany { get; set; }
    }
}
