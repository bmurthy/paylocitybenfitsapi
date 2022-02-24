using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using paylocitybenfitsapi;
using paylocitybenfitsapi.Repository;
using paylocitybenfitsapi.Services;
using paylocitybenfitsapi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: WebJobsStartup(typeof(Startup))]
namespace paylocitybenfitsapi
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.TryAddTransient<IPaylocityDatabase, PaylocityDatabase>();
            builder.Services.TryAddTransient<IEmployeeRepository, EmployeeRepository>();
            builder.Services.TryAddTransient<IEmployeeService, EmployeeService>();
            builder.Services.TryAddTransient<IPayCalculationService, PayCalculationService>();
            builder.Services.TryAddTransient<IBenefitsService, BenefitsService>();
            builder.Services.TryAddTransient<IEmployeeValidator, EmployeeValidator>();
        }
    }
}
