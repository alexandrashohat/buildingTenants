using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BuildingTenants
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }


    public class connectionstr
    {
        private string _connectionString;
        public connectionstr(IConfiguration configuration)
        {
            Configuration = configuration;
            _connectionString = Configuration.GetConnectionString("defaultConnection");
        }

        public IConfiguration Configuration { get; }
        public List<string> getTenants()
        {
            var ret = new List<String>();
            try
            {
                using (SqlConnection connection = new SqlConnection(
                _connectionString))
                {
                    SqlCommand command = new SqlCommand("select * from tenant", connection);
                    command.CommandType = System.Data.CommandType.TableDirect;
                    command.Connection.Open();
                    SqlDataReader read =  command.ExecuteReader();
                    while (read.Read())
                    {
                        ret.Add("rrr");
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return new List<string>();
        }
    }
}
