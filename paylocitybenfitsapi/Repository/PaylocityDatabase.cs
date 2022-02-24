using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paylocitybenfitsapi.Repository
{
    public class PaylocityDatabase : IPaylocityDatabase
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("PaylocityDatabaseConnection");
        
        public IDbConnection CreateConnection()
            => new SqlConnection(connectionString);
    }
}
