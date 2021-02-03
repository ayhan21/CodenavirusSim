using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CodenavirusSim.Data
{
    /*
     * Connects to database and returns SqlConnection object for Db operations
     */
    public static class DBConnect
    {
        public static SqlConnection Connect()
        {
            SqlConnection con;
            string connectionString;
            connectionString = @"Data Source=DESKTOP-VURFUTQ;Initial Catalog=SimDB; Integrated Security=SSPI";
            con = new SqlConnection(connectionString);

            try
            {
                con.Open();
            }
            catch (SqlException)
            {
            }

            return con;
        }
    }
}
