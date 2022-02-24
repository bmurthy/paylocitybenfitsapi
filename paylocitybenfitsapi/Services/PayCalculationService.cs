using paylocitybenfitsapi.Common;
using paylocitybenfitsapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Services
{
    public class PayCalculationService: IPayCalculationService
    {
        private readonly IBenefitsService benefitsService;

        public PayCalculationService(IBenefitsService benefitsService)
        {
            this.benefitsService = benefitsService;
        }

        public EmployeeCostToCompany Calculate(Employee employee)
        {
            try
            {
                EmployeeCostToCompany employeeCostToCompany = MapNames(employee);

                var costOfBenefits = benefitsService.GetEmployeeBenefits(employee.FirstName) + benefitsService.GetDependentBenfitsCost(employee.Dependents);

                var salary = CalculateSalary();

                var costToCompany = salary + costOfBenefits;

                employeeCostToCompany.EmployeeId = new Random().Next();

                employeeCostToCompany.Benefits = costOfBenefits;

                employeeCostToCompany.Salary = salary;

                employeeCostToCompany.CostToCompany = costToCompany;

                return employeeCostToCompany;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        //This is not good interms of domain model. :) 
        private EmployeeCostToCompany MapNames(Employee employee)
        {
            EmployeeCostToCompany employeeCostToCompany = new EmployeeCostToCompany();
            employeeCostToCompany.FirstName = employee.FirstName;
            employeeCostToCompany.LastName = employee.LastName;
            employeeCostToCompany.DependentName1 = employee.Dependents[0].Name;
            employeeCostToCompany.DependentName2 = employee.Dependents[1].Name;
            employeeCostToCompany.DependentName3 = employee.Dependents[2].Name;
            employeeCostToCompany.DependentName4 = employee.Dependents[3].Name;
            employeeCostToCompany.Relation1 = employee.Dependents[0].Relation;
            employeeCostToCompany.Relation2 = employee.Dependents[1].Relation;
            employeeCostToCompany.Relation3 = employee.Dependents[2].Relation;
            employeeCostToCompany.Relation4 = employee.Dependents[3].Relation;


            return employeeCostToCompany;
        }


        private double CalculateSalary()
        {
            return PayloCityConstants.EmployeeSalary * PayloCityConstants.NumberOfPayChecks;
        }
    }
}
