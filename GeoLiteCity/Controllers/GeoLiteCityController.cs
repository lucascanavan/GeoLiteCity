using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GeoLiteCity.Controllers
{
    public class GeoLiteCityController : ApiController
    {
        public object Get(string ip = "")
        {
            var countryCode = "AU";
            var region = "02";
            var city = "Sydney";

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }

            string connString = ConfigurationManager.ConnectionStrings["GeoLiteCity"].ConnectionString;

            using (var conn = new SqlConnection(connString))
            {
                string sql = @"SELECT TOP 1 @countryCode = Country, @region = region, @city = city FROM [dbo].[GeoLiteCity-Location] l
                                JOIN [dbo].[GeoLiteCity-Blocks] b ON b.LocId = l.LocId
                                WHERE @ipAsLong BETWEEN b.StartIpNum AND b.EndIpNum";

                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.Add("@ipAsLong", SqlDbType.BigInt);
                    long ipAsLong = IPAddressToLong(ip);
                    cmd.Parameters["@ipAsLong"].Value = ipAsLong;

                    SqlParameter countryCodeParam = new SqlParameter("@countryCode", SqlDbType.NChar);  
                    countryCodeParam.Size = 2;  
                    countryCodeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(countryCodeParam);

                    SqlParameter regionParam = new SqlParameter("@region", SqlDbType.NChar);
                    regionParam.Size = 2;
                    regionParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(regionParam);

                    SqlParameter cityParam = new SqlParameter("@city", SqlDbType.NVarChar);
                    cityParam.Size = 255;
                    cityParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(cityParam);  

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    countryCode = countryCodeParam.Value.ToString();
                    region = regionParam.Value.ToString();
                    city = cityParam.Value.ToString();

                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

            return new { IP = ip, CountryCode = countryCode, Region = region, City = city };
        }

        private long IPAddressToLong(string IPAddr)
        {
            System.Net.IPAddress oIP = System.Net.IPAddress.Parse(IPAddr);

            long ipnum = 0;
            byte[] b = oIP.GetAddressBytes();
            for (int i = 0; i < 4; ++i)
            {
                long y = b[i];
                if (y < 0)
                {
                    y += 256;
                }
                ipnum += y << ((3 - i) * 8);
            }

            return ipnum;
        }
    }
}