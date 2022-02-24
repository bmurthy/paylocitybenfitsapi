using Moq;
using paylocitybenfitsapi.Models;
using paylocitybenfitsapi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaylocityApi.Tests
{
    public class CalculationServiceTests
    {
        [Theory]
        [MemberData(nameof(GetEmployeeDataWithBenefits))]
        public void CheckCostToCompany(Employee employee, double benefits, double expectedResult)
        {
            var benefitService = new Mock<IBenefitsService>();
            benefitService.Setup(x => x.GetEmployeeBenefits(It.IsAny<string>())).Returns(benefits);
            var calculationService = new PayCalculationService(benefitService.Object);


            // Act
            var result = calculationService.Calculate(employee);

            // Assert
            Assert.Equal(expectedResult, result.CostToCompany);
        }

        public static IEnumerable<object[]> GetEmployeeDataWithBenefits()
        {
            yield return new object[]
            {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent(){Name = "Ana"} } },
                1450,53450
            };

            yield return new object[]
            {
                new Employee(){FirstName="Andrew" },
                900,52900
            };

            yield return new object[]
             {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent() { Name = "Ana" }, new Dependent() { Name = "Jon" }  } },
                1950,53950
             };

            yield return new object[]
            {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent() { Name = "Tom" },
                    new Dependent() { Name = "Jon" }, new Dependent() { Name = "Rob" }, new Dependent() { Name = "Ken" } } },
                3000,55000
            };

            yield return new object[]
            {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent() { Name = "Ana" },
                    new Dependent() { Name = "Jon" }, new Dependent() { Name = "Rob" }, new Dependent() { Name = "Arnold" } } },
                2900,54901
            };
        }
    }
}
