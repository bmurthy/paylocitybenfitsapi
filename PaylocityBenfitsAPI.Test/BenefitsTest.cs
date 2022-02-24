using paylocitybenfitsapi.Models;
using paylocitybenfitsapi.Services;
using System.Collections.Generic;
using Xunit;


namespace PaylocityBenfitsAPI.Test
{
    public class BenefitsTest
    {
        public class BenefitsTests
        {
            [Theory]
            [MemberData(nameof(GetEmployeeData))]
            public void CheckEmployeeBenefits(string employeeName, double expectedResult)
            {
                var benefitService = new BenefitsService();

                // Act
                var result = benefitService.GetEmployeeBenefits(employeeName);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            public static IEnumerable<object[]> GetEmployeeData()
            {
                yield return new object[]
                {
                "Jon",
                1000
                };

                yield return new object[]
                {
                "Andrew" ,
                900
                };


            }

            [Theory]
            [MemberData(nameof(GetEmployeeDataWithBenefits))]
            public void CheckDependentBenefits(Employee employee, double expectedResult)
            {
                var benefitService = new BenefitsService();

                // Act
                var result = benefitService.GetDependentBenfitsCost(employee.Dependents);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            public static IEnumerable<object[]> GetEmployeeDataWithBenefits()
            {
                yield return new object[]
                {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent(){Name = "Ana"} } },
                450
                };

                yield return new object[]
                {
                new Employee(){FirstName="Andrew" },
                0
                };

                yield return new object[]
                 {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent() { Name = "Ana" }, new Dependent() { Name = "Jon" }  } },
                950
                 };

                yield return new object[]
                {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent() { Name = "Tom" },
                    new Dependent() { Name = "Jon" }, new Dependent() { Name = "Rob" }, new Dependent() { Name = "Ken" } } },
                2000
                };

                yield return new object[]
                {
                new Employee(){FirstName="John", Dependents = new List<Dependent>(){ new Dependent() { Name = "Ana" },
                    new Dependent() { Name = "Jon" }, new Dependent() { Name = "Rob" }, new Dependent() { Name = "Arnold" } } },
                1900
                };
            }
        }
    }
}