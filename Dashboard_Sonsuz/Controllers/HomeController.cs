﻿using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Localization;

namespace Dashboard.Controllers
{

    public class HomeController : BaseController
    {

        private List<Dictionary<string, long>> list = new List<Dictionary<string, long>>();
        private List<long> dateList = new List<long>();
        public HomeController(Context context, IConfiguration config) : base(context, config) { }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
        {
            DateTime time = DateTime.Now;
            for (int i = 1; i <= 14; i++)
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
                long secondsSinceEpoch = (long)t.TotalMilliseconds;
                dateList.Add(secondsSinceEpoch);
                list.Add(d);
                time = time.AddDays(-1);

            }
            ViewBag.chartData = list;

            string strDateData = "[";
            for (int i = 0; i < dateList.Count; i++)
            {
                if (i != dateList.Count - 1) strDateData += "new Date(" + getNewEpoch(dateList[i].ToString()) + ")" + ",";
                else strDateData += "new Date(" + getNewEpoch(dateList[i].ToString()) + ")";
            }
            strDateData += "]";

            ViewBag.dateData = strDateData;


            ViewBag.branchCount = _context.branch.Count();
            ViewBag.userCount = _context.user.Count();
            ViewBag.areaCount = _context.areaInfo.Count();


            List<BranchTransActions> transActionsData = _context.branchTransActions.Where(x => x.statusId == 1).Include(x => x.card).Include(x => x.status).OrderByDescending(x => x.transId).Take(5).ToList();
            ViewBag.transActionsData = transActionsData;
            List<DepositTransActions> depositTransActions = _context.depositTransActions.Where(x => x.statusId == 1).Include(x => x.branch).Include(x => x.status).OrderByDescending(x => x.transId).Take(5).ToList();
            ViewBag.depositTransActions = depositTransActions;

            return View();
        }

        public IActionResult ApproveNotification(long notificationId)
        {
            AdminNotification notification = _context.adminNotification.Find(notificationId);
            notification.active = true;
            _context.adminNotification.Update(notification);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private string getNewEpoch(string getValue)
        {
            string rtValue = "";
            int i = 0;
            int j = 0;
            foreach (char c in getValue)
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
