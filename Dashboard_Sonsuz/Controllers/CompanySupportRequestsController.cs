﻿using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class CompanySupportRequestsController : Controller
    {

        private readonly Context _context;
        private Admin admin;
        private readonly IConfiguration _config;
        public CompanySupportRequestsController(Context context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }

        [HttpPost]
        public ActionResult PartialViewEdit(long requestId)
        {
            CompanySupportRequests request = _context.companySupportRequests.Find(requestId);
            return PartialView("PartialViewEdit", request);
        }

        [HttpPost]
        public async Task<IActionResult> SendSupportMessageAsync(string message, long requestId)
        {
            CompanySupportRequests request = _context.companySupportRequests
                .Where(x => x.requestId == requestId)
                .Include(x => x.branch)
                .Include(x => x.branch.contact).ToList()[0];
            DateTime time = DateTime.Now;
            string format = "dd/M/yyyy";
            _context.branchNotifications.Add(
               new BranchNotifications
               {
                   branchId = request.branchId,
                   content = message,
                   date = time.ToString(format),
                   header = "Destek Talebi Hakkında",
                   isRead = false,
                   sender = "BiMaçVar!"
               });


            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "DESTEK TALEBİNİZ CEVAPLANDI";
            email.content = message;
            email.mail = request.branch.contact.mail;
            await email.sendAsync(_config);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }




        public async Task<IActionResult> IndexAsync()
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            List<CompanySupportRequests> requests = _context.companySupportRequests.Include(x => x.branch).ToList();

            return View(requests);
        }
    }
}
