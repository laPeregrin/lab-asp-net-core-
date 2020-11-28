using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace BO_BusinessObjects_.ORMs.Services
{
    public class BasedDAO
    {
        //private readonly string connStrReader;
        public SqlConnection conn;
        public  SqlCommand command;

        public BasedDAO()
        {
           // connStrReader = ConfigurationManager.ConnectionStrings["DevConnection"]
            //    .ConnectionString;
            conn = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB; Database = Contracts_db; Persist Security Info = false; MultipleActiveResultSets = True; Trusted_Connection = True;");
            command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            OpenConnection();
        }

        private SqlConnection OpenConnection()
        {
            if(conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }
    }
}
