using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace access_data_from_the_database
{
    public class Program
    {
        private static string _connectionString = "Data Source=10.20.32.3;Initial Catalog=NovaDB;Persist Security Info=True;User ID=sa;Password=d7*FT+ef)_n+";
        private static string _localConnectionString = "Data Source=192.168.0.56;Initial Catalog=NovaDB;Persist Security Info=True;User ID=sa;Password=d7*FT+ef)_n+";
        public static void Main()
        {
            //DataResults();
            //StoreProcedureDataResults();
            StoreProcedureByIdDataResults();
        }

        private static List<object> AccessDataWithSqlConnection()
        {
            var data = new List<object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from Ops.Region", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        data.Add(new
                        {
                            RegionId = Convert.ToInt32(reader[0]),
                            RegionCode = reader[1].ToString(),
                            RegionName = reader[2].ToString(),
                            Dormant = Convert.ToBoolean(reader[3]),
                            Date = Convert.ToDateTime(reader[5])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return data;
        }

        private static List<object> AccessDataWithStoreProcedure()
        {
            var data = new List<object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_localConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.spGetAllRegions", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        data.Add(new
                        {
                            RegionId = Convert.ToInt32(reader[0]),
                            RegionCode = reader[1].ToString(),
                            RegionName = reader[2].ToString(),
                            Dormant = Convert.ToBoolean(reader[3]),
                            Date = Convert.ToDateTime(reader[5])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return data;
        }

        private static List<object> AccessDataWithStoreProcedureById()
        {
            var data = new List<object>();

            try
            {
                using (SqlConnection con = new SqlConnection(_localConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.spGetRegionsPerId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    Console.WriteLine("Enter Region Id:");
                    string RegionId = Console.ReadLine();
                    cmd.Parameters.Add("@RegionId", SqlDbType.Int).Value = Convert.ToInt32(RegionId);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        data.Add(new
                        {
                            RegionId = Convert.ToInt32(reader[0]),
                            RegionCode = reader[1].ToString(),
                            RegionName = reader[2].ToString(),
                            Dormant = Convert.ToBoolean(reader[3]),
                            Date = Convert.ToDateTime(reader[5])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return data;
        }

        private static void DataResults()
        {
            var results = AccessDataWithSqlConnection();

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static void StoreProcedureDataResults()
        {
            var results = AccessDataWithStoreProcedure();

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static void StoreProcedureByIdDataResults()
        {
            var results = AccessDataWithStoreProcedureById();

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
