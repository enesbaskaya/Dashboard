using Dashboard_Sonsuz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Dashboard_Sonsuz.Controllers
{
    public class HomeController : Controller
    {
        Admin admin;
        List<Dictionary<string, long>> list = new List<Dictionary<string, long>>();
        List<long> dateList = new List<long>();

        private readonly Context _context;

        public HomeController(Context context)
        {
       
            _context = context;
        }

        public async System.Threading.Tasks.Task<IActionResult> IndexAsync()
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));

            DateTime time = DateTime.Now;
            for (int i=1;i<=14;i++)
            {
                string dayStr = time.Year + "-" + time.Month + "-" + time.Day;

                int branchCount = _context.branch.Where(x => x.registerDate.Equals(dayStr)).Count();
                int userCount = _context.user.Where(x => x.registerDate.Equals(dayStr)).Count();
                int areaCount = _context.areaInfo.Where(x => x.registerDate.Equals(dayStr)).Count();
                Dictionary<string, long> d = new Dictionary<string, long>();
                d.Add("B", branchCount);
                d.Add("U", userCount);
                d.Add("A", areaCount);
                TimeSpan t = time - new DateTime(1970, 1, 1);
                long secondsSinceEpoch = (long) t.TotalMilliseconds;
                ViewBag.data = secondsSinceEpoch;
                dateList.Add(secondsSinceEpoch);
                list.Add(d);
                time = time.AddDays(-1);
                
           }
            ViewBag.chartData = list;

            string strDateData = "[";
            for (int i=0;i<dateList.Count;i++)
            {
                if(i != dateList.Count -1) strDateData += "new Date(" + getNewEpoch(dateList[i].ToString()) + ")" + ",";
                else strDateData += "new Date(" + getNewEpoch(dateList[i].ToString()) + ")";
            }
            strDateData += "]";

            ViewBag.dateData = strDateData;


            ViewBag.branchCount = _context.branch.Count();
            ViewBag.userCount = _context.user.Count();
            ViewBag.areaCount = _context.areaInfo.Count();



            return View(this.admin);
        }


        private string getNewEpoch(string getValue)
        {
            string rtValue="";
            int i = 0;
            int j = 0;
            foreach(char c in getValue)
            {
                if (i < 7)
                    rtValue += c.ToString();
                else
                    j++;
                i++;
            }

            rtValue += "e" + j.ToString();

            return rtValue;
        }

    }
}
