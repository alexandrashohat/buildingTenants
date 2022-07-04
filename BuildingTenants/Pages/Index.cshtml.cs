using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BuildingTenants.serviceClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace BuildingTenants.Pages
{
    public class IndexModel : PageModel
    {
        public int numberOfTenants;
        public string BuildingName;
        public List<tenantInfo> listOfTenents = new List<tenantInfo>();
        public List<GuestInfo> guestInfoList = new List<GuestInfo>();
        public List<(int, int)> LstnumberOfTenants = new List<(int, int)>();
        public void OnGet()
        {
            
            try
            {
                String connectionStr = "Data Source=SASHA-PC;Initial Catalog=building;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("SELECT [apartment_id],[original_number_of_tenants],[current_number_of_tenants]"
                                        +",[start_date],[end_date],[building_id] FROM[building].[dbo].[Tenant_state]", conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listOfTenents.Add(new tenantInfo()
                                {
                                   
                                    start_date = Convert.ToDateTime(reader["start_date"].ToString()).ToShortDateString(),
                                    end_date = Convert.ToDateTime(reader["end_date"].ToString()).ToShortDateString(),
                                    apartment_id = Convert.ToInt32(reader["apartment_id"].ToString()),
                                    building_id = Convert.ToInt32(reader["building_id"].ToString()),
                                    original_number_of_tenants = Convert.ToInt32(reader["original_number_of_tenants"].ToString()),
                                    current_number_of_tenants = Convert.ToInt32(reader["current_number_of_tenants"].ToString()),

                                });
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand("SELECT apartment_id,ISNULL(number_of_guests,0) number_of_guests,current_number_of_tenants,[date]"
                        +"FROM guests g right join Tenant_state ts on g.apartment_number = ts.apartment_id;", conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                guestInfoList.Add(new GuestInfo()
                                {

                                    id = reader["apartment_id"].ToString(),
                                    number_of_guests = Convert.ToInt32(reader["number_of_guests"].ToString()),
                                    current_number_of_tenants = Convert.ToInt32(reader["current_number_of_tenants"].ToString())

                                });
                            }
                        }
                    }

                }
                 LstnumberOfTenants = listOfTenents.GroupBy(v => v.building_id)
                    .Select(a => ( a.Select(c =>c.building_id).FirstOrDefault(), a.Select(c => c.current_number_of_tenants).Sum() )).ToList();
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
    public class tenantInfo
    {   
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int building_id { get; set; }
        public int original_number_of_tenants { get; set; }
        public int current_number_of_tenants { get; set; }
        public int apartment_id { get; set; }

    }
    public class GuestInfo
    {
        public int current_number_of_tenants { get; set; }
        public string id { get; set; }
        public int number_of_guests { get; set; }

    }
}
