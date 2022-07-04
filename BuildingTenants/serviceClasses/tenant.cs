using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingTenants.serviceClasses
{
    public class tenant
    {
        public int tenant_id { get; set; }
        public string tenant_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string building_name { get; set; }
        public int apartment_number { get; set; }

    }
    public class tenants {
        private string _connectionString;
        public tenants(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public List<tenant> getTenants()
        {
            var ret = new List<tenant>();
            try
            {
                using (SqlConnection connection = new SqlConnection(
                _connectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT [tenant_id] ,[tenant_name] ,[start_date]"
                                        +  ",[end_date] ,[building_name] ,[apartment_number] FROM[building].[dbo].[tenant]", connection);
                  //  command.CommandType = System.Data.CommandType.TableDirect;
                    command.Connection.Open();
                    SqlDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        ret.Add(new tenant()
                        {
                            building_name =  read["building_name"].ToString(),
                            tenant_id = Convert.ToInt32(read["tenant_id"].ToString()),
                            tenant_name = read["tenant_name"].ToString(),
                            start_date = Convert.ToDateTime(read["start_date"].ToString()),
                            end_date = Convert.ToDateTime(read["end_date"].ToString()),
                            apartment_number = Convert.ToInt32(read["apartment_number"].ToString())

                        });
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ret;
        }
    }
}
