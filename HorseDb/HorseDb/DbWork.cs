using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseDb
{
    class DbWork
    {
        public SqlConnection Connect()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"USER\MSSQLSERVER1";
            builder.IntegratedSecurity = true;

            return new SqlConnection(builder.ConnectionString);
        }
    }
}
