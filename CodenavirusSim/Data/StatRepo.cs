using CodenavirusSim.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodenavirusSim.Data
{
    /* Perforns insert and select queries to/from database
     */
    public class StatRepo : IDbRepo
    {
        /* Gets Simulation Statistics by ID of simulation
         */
        public IEnumerable<Stat> GetSimStats(int simId)
        {
            List<Stat> statsList = new List<Stat>();

            string query = "SELECT days, infected_count, recovered_count, healthy_count FROM sim_results WHERE sim_id = @id";

            try
            {
                SqlConnection conn = DBConnect.Connect();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = simId;

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        statsList.Add(new Stat(
                            dr.GetInt32(0),
                            dr.GetInt32(1),
                            dr.GetInt32(2),
                            dr.GetInt32(3)
                        ));
                    }

                    conn.Close();
                }
            }
            catch (SqlException)
            {
                
            }

            return statsList;
        }

        /* Insert a given Simulation's input data to the database (table input_data)
         */
        public void InsertInputData(Simulation sim)
        {
            string insertQuery = "INSERT INTO input_data (matrix, first_infected) VALUES (@m, @fi) SELECT @@IDENTITY";

            try
            {
                SqlConnection con = DBConnect.Connect();
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.Add("@m", SqlDbType.NChar).Value = toStr(sim.InputScale);
                    cmd.Parameters.Add("@fi", SqlDbType.NChar).Value = string.Join(",", sim.FirstInfected);
                    

                    // Gets id of the added row
                    SqlDataReader dr = cmd.ExecuteReader();
                    decimal id;
                    while (dr.Read())
                    {
                        id = dr.GetDecimal(0);
                        sim.SimId = int.Parse(dr.GetDecimal(0).ToString());
                    }

                    con.Close();
                }

            }
            catch (SqlException)
            {
            }
        }

        // Transforms 2d array to string
        private string toStr(string[,] a)
        {
            var sb = new StringBuilder(string.Empty);
            var maxI = a.GetLength(0);
            var maxJ = a.GetLength(1);
            for (var i = 0; i < maxI; i++)
            {
                sb.Append(",{");
                for (var j = 0; j < maxJ; j++)
                {
                    sb.Append($"{a[i, j]},");
                }

                sb.Append("}");
            }

            sb.Replace(",}", "}").Remove(0, 1);
            return sb.ToString();
        }

        /* Inserts simulation results in database using the ID of the given simulation
         */
        public void InsertStats(Simulation sim)
        {
            string insertQuery = "INSERT INTO sim_results (sim_id, days, infected_count, recovered_count, healthy_count) VALUES (@id, @days, @i, @r, @h)";

            try
            {
                SqlConnection con = DBConnect.Connect();
                foreach (var stat in sim.Stats)
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = sim.SimId;
                        cmd.Parameters.Add("@days", SqlDbType.Int).Value = stat.Day;
                        cmd.Parameters.Add("@i", SqlDbType.Int).Value = stat.InfectedCount;
                        cmd.Parameters.Add("@r", SqlDbType.Int).Value = stat.RecoveredCount;
                        cmd.Parameters.Add("@h", SqlDbType.Int).Value = stat.HealthyCount;

                        cmd.ExecuteNonQuery();
                    }
                }
            
                con.Close();

            }
            catch (SqlException)
            {
            }
        }
    }
}
