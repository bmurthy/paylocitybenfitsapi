using paylocitybenfitsapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace paylocitybenfitsapi.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IPaylocityDatabase paylocityDatabase;

        public EmployeeRepository(IPaylocityDatabase paylocityDatabase)
        {
            this.paylocityDatabase = paylocityDatabase;
        }
        private List<EmployeeCostToCompany> MapEmployeeDetail(DataTable dt)
        {
            List<EmployeeCostToCompany> employeedetails = new List<EmployeeCostToCompany>();
            EmployeeCostToCompany employee;
            var employeeNumbers = (from r in dt.AsEnumerable()
                     select r["EmployeeId"]).Distinct().ToList();
            foreach (int employeeNumber in employeeNumbers)
            {
                employee = new EmployeeCostToCompany();
                foreach (DataRow dr in dt.Rows)
                {
                    
                    if (dr.Field<int>("EmployeeId") == employeeNumber)
                    {
                        employee.FirstName = dr.Field<string>("EmployeeFirstName");
                        employee.LastName = dr.Field<string>("EmployeeLastName");
                        employee.EmployeeId = dr.Field<int>("EmployeeId");
                        employee.Salary = (double)dr.Field<decimal>("Salary");
                        employee.Benefits = (double)dr.Field<decimal>("Benefits");
                        employee.CostToCompany = (double)dr.Field<decimal>("CostToCompany");
                        if (dr["DependentNumber"] != DBNull.Value)
                        {
                            switch (dr.Field<int>("DependentNumber"))
                            {
                                case 1:
                                    employee.DependentName1 = dr.Field<string>("DependentName");
                                    employee.Relation1 = dr.Field<string>("RelationShip");
                                    break;

                                case 2:
                                    employee.DependentName2 = dr.Field<string>("DependentName");
                                    employee.Relation2 = dr.Field<string>("RelationShip");
                                    break;
                                case 3:
                                    employee.DependentName3 = dr.Field<string>("DependentName");
                                    employee.Relation3 = dr.Field<string>("RelationShip");
                                    break;
                                case 4:
                                    employee.DependentName4 = dr.Field<string>("DependentName");
                                    employee.Relation4 = dr.Field<string>("RelationShip");
                                    break;
                            }
                        }
                    }
                    
                }
                employeedetails.Add(employee);

            }


            return employeedetails;
        }
        public async Task<IEnumerable<EmployeeCostToCompany>> GetEmployees()
        {
            List<EmployeeCostToCompany> employeedetails = new List<EmployeeCostToCompany>();
            try
            {
                
                DataTable dt = new DataTable();
                string connectionString = Environment.GetEnvironmentVariable("PaylocityDatabaseConnection");
                
                var query = @"select e.EmployeeId,e.EmployeeFirstName,e.EmployeeLastName,e.NumberofDependents,dd.DependentName,dd.DependentNumber, dd.RelationShip,
                                bs.Salary, bs.Benefits, bs.CostToCompany
                                from Employee e left join DependentDetails dd on e.employeeId = dd.employeeid
                                inner join BenefitSummary bs on e.employeeId = bs.employeeid
                                order by e.EmployeeFirstName,e.EmployeeLastName";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = connection;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
                        }
                        connection.Close();
                    }
                    //var dt = await connection.QueryAsync<DataTable>(query);
                    if (dt.Rows.Count > 0)
                    {
                        employeedetails = MapEmployeeDetail(dt);
                    }
                    //connection.Close();

                }
                
                
                
            }
            catch (Exception ex)
            {
                throw;
            }
            return employeedetails.ToList();
        }
        private List<Dependent> MapDependents (EmployeeCostToCompany employee)
        {
            List<Dependent> dependentList = new List<Dependent>();
            Dependent dependent;
            if(!string.IsNullOrEmpty(employee.DependentName1))
            {
                dependent = new Dependent();
                dependent.DependenNumber = 1;
                dependent.Name = employee.DependentName1;
                dependent.Relation = employee.Relation1;
                dependentList.Add(dependent);
            }
            if(!string.IsNullOrEmpty(employee.DependentName2))
            {
                dependent = new Dependent();
                dependent.DependenNumber = 2;
                dependent.Name = employee.DependentName2;
                dependent.Relation = employee.Relation2;
                dependentList.Add(dependent);
            }
            if (!string.IsNullOrEmpty(employee.DependentName3))
            {
                dependent = new Dependent();
                dependent.DependenNumber = 3;
                dependent.Name = employee.DependentName3;
                dependent.Relation = employee.Relation3;
                dependentList.Add(dependent);
            }
            if (!string.IsNullOrEmpty(employee.DependentName4))
            {
                dependent = new Dependent();
                dependent.DependenNumber = 4;
                dependent.Name = employee.DependentName4;
                dependent.Relation = employee.Relation4;
                dependentList.Add(dependent);
            }

            return dependentList;
        }

        public async Task SaveEmployee(EmployeeCostToCompany employeeCostToCompany)
        {
            string query = string.Empty;

            try
            {
                DynamicParameters parameters;

                List<Dependent> dependentList = MapDependents(employeeCostToCompany);
                query = @"INSERT INTO [dbo].[Employee]([EmployeeId],[EmployeeFirstName],[EmployeeLastName],[NumberofDependents],[ModifiedUser],[ModifiedDateTime])
     VALUES
           (@EmployeeId
           ,@EmployeeFirstName
           ,@EmployeeLastName
           ,@NumberofDependents
           ,@ModifiedUser
           ,@ModifiedDateTime)";
                parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeCostToCompany.EmployeeId, DbType.Int32);
                parameters.Add("EmployeeFirstName", employeeCostToCompany.FirstName, DbType.String);
                parameters.Add("EmployeeLastName", employeeCostToCompany.LastName, DbType.String);
                parameters.Add("NumberofDependents", 0, DbType.Int32);
                parameters.Add("ModifiedUser", "bmurthy", DbType.String);
                parameters.Add("ModifiedDateTime", DateTime.Now, DbType.DateTime);
                using (var connection = paylocityDatabase.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
                query = @"INSERT INTO [dbo].[BenefitSummary]([EmployeeId],[Salary],[Benefits],[CostToCompany],[ModifiedUser],[ModifiedDateTime])
     VALUES
           (@EmployeeId
           ,@Salary
           ,@Benefits
           ,@CostToCompany
           ,@ModifiedUser
           ,@ModifiedDateTime)";
                parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeCostToCompany.EmployeeId, DbType.Int32);
                parameters.Add("Salary", employeeCostToCompany.Salary, DbType.Decimal);
                parameters.Add("Benefits", employeeCostToCompany.Benefits, DbType.Decimal);
                parameters.Add("CostToCompany", employeeCostToCompany.CostToCompany, DbType.Decimal);
                parameters.Add("ModifiedUser", "bmurthy", DbType.String);
                parameters.Add("ModifiedDateTime", DateTime.Now, DbType.DateTime);
                using (var connection = paylocityDatabase.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
                if (dependentList.Count() > 0)
                {
                    foreach (var dependent in dependentList)
                    {

                        query = @"INSERT INTO [dbo].[DependentDetails]([EmployeeId],[DependentName],[Relationship],[DependentNumber],[ModifiedUser],[ModifiedDateTime])
     VALUES
           (@EmployeeId
           ,@DependentName
           ,@Relationship   
            ,@DependentNumber
           ,@ModifiedUser
           ,@ModifiedDateTime)";
                        parameters = new DynamicParameters();
                        parameters.Add("EmployeeId", employeeCostToCompany.EmployeeId, DbType.Int32);
                        parameters.Add("DependentName", dependent.Name, DbType.String);
                        parameters.Add("Relationship", dependent.Relation, DbType.String);
                        parameters.Add("DependentNumber", dependent.DependenNumber, DbType.Int32);
                        parameters.Add("ModifiedUser", "bmurthy", DbType.String);
                        parameters.Add("ModifiedDateTime", DateTime.Now, DbType.DateTime);
                        using (var connection = paylocityDatabase.CreateConnection())
                        {
                            await connection.ExecuteAsync(query, parameters);
                        }


                    }
                }
            }

            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
