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
    public class DeleteAreaRequestsController : Controller
    {

        private readonly Context _context;
        private Admin admin;
        private readonly IConfiguration _config;
        public DeleteAreaRequestsController(Context context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }


        public async Task<IActionResult> ApproveDeleteArea(long requestId)
        {
            DeleteAreaRequests deleteArea = _context.deleteAreaRequests
                .Where(x => x.deleteRequestId == requestId)
                .Include(x => x.area)
                .Include(x => x.area.branch)
                .Include(x => x.area.branch.contact)
                .ToList()[0];


            String message = "Değerli BiMaçVar! üyemiz, talepte bulunduğunuz saha silme talebi gerçekeştirildi ve " + deleteArea.area.areaName + " adlı sahanız kaldırıldı!";

            DateTime time = DateTime.Now;
            string format = "dd/M/yyyy";
            _context.branchNotifications.Add(
               new BranchNotifications
               {
                   branchId = deleteArea.area.branchId,
                   content = message,
                   date = time.ToString(format),
                   header = "Saha Silme Talebi",
                   isRead = false,
                   sender = "BiMaçVar!"
               });


            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "SAHANIZ İSTEĞİNİZ ÜZERE SİLİNDİ";
            email.content = message;
            email.mail = deleteArea.area.branch.contact.mail;
            await email.sendAsync(_config);

            _context.areaInfo.Remove(deleteArea.area);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IndexAsync()
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            List<DeleteAreaRequests> requests = _context.deleteAreaRequests.Include(x => x.area).ToList();
            return View(requests);
        }
    }
}
