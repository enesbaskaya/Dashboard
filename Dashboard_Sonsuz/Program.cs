using Dashboard.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard
{
    public class Program
    {

        public static Admin administrator;

        public static string getDotColor(long statusId)
        {
            switch (statusId)
            {
                case 1:
                    return "warning"; // TURUNCU
                case 2:
                    return "success"; // YEÞÝL
                case 3:
                    return "primary"; // KOYU MAVÝ
                case 4:
                    return "danger"; // KIRMIZI
                default:
                    return "info"; // TURKUVAZ
            }
        }

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
