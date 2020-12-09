using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard_Sonsuz
{
    public class Program
    {

        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
