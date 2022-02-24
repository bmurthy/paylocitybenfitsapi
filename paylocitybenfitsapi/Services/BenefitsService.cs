using paylocitybenfitsapi.Common;
using paylocitybenfitsapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Services
{
    public class BenefitsService : IBenefitsService
    {
        public double GetDependentBenfitsCost(List<Dependent> dependents)
        {
            if(dependents == null || dependents.Count ==0)
                return 0;

            int countWithANames = dependents.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().StartsWith(PayloCityConstants.SmallA)).Count();
            
            int countOthers = dependents.Where(x=> !string.IsNullOrEmpty(x.Name)).Count() - countWithANames;

            return GetDependentBenefits(countOthers) + GetDependentBenefitsNameStartsWithA(countWithANames);
        }

        private double GetDependentBenefits(int count)
        {
            return count * PayloCityConstants.DependentBenefitCost;
        }

        private double GetDependentBenefitsNameStartsWithA(int count)
        {
            return count * (PayloCityConstants.DependentBenefitCost -(PayloCityConstants.DependentBenefitCost * PayloCityConstants.DiscountPercent));
        }

        public double GetEmployeeBenefits(string employeeName)
        {
            if (employeeName.ToLower().StartsWith(PayloCityConstants.SmallA))
            {
                return (PayloCityConstants.EmployeeBenefitCost - (PayloCityConstants.EmployeeBenefitCost * PayloCityConstants.DiscountPercent));
            }
            else
            {
                return PayloCityConstants.EmployeeBenefitCost;
            }
        }
    }
}
