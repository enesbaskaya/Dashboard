﻿using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class BranchApplicationController : Controller
    {
        private Admin admin;

        private readonly Context _context;

        public BranchApplicationController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            List<Branch> deActiveBranches = (from b in _context.branch
                                             join c in _context.contactInfo on b.contactId equals c.contactId
                                             join d in _context.districts on c.districtId equals d.districtId
                                             join ct in _context.city on d.cityId equals ct.cityId
                                             where !b.isActive
                                             select new Branch
                                             {
                                                 branchId = b.branchId,
                                                 admin = b.admin,
                                                 name = b.name,
                                                 isActive = b.isActive,
                                                 taxNumber = b.taxNumber,
                                                 registerDate = b.registerDate,
                                                 contact = new ContactInfo
                                                 {
                                                     mail = c.mail,
                                                     telephone = c.telephone,
                                                     district = new Districts
                                                     {
                                                         districtName = d.districtName,
                                                         city = new City
                                                         {
                                                             cityName = ct.cityName,
                                                         }
                                                     }
                                                 }
                                             }
                                             ).ToList();
            return View(deActiveBranches);
        }


        public async Task<IActionResult> ApproveBranch(long branchId)
        {

            Branch updatedData = await _context.branch.FindAsync(branchId);

            updatedData.isActive = true;
            _context.branch.Update(updatedData);


            string content = "Sayın, " + updatedData.admin + "\n'BiMaçVar! ailesine hoşgeldiniz! BiMaçVar! olarak müşterilerinize daha kolay erişebilecek ve onlarla" +
            " çok daha rahat etkileşimde bulunabileceksiniz! Ayrıca rekabeti artıran bir çok sistemin mevcut olması sebebiyle potansiyelinizi burada keşfetmenize ve en iyi halısaha olma yolunda" +
            "sizlerin yanında olacağız!";

            string header = "Aramıza Hoşgeldiniz!";
            DateTime time = DateTime.Now;
            string format = "dd/M/yyyy";
            var insertNotification = await _context.branchNotifications.AddAsync(
                new BranchNotifications
                {
                    branchId = updatedData.branchId,
                    content = content,
                    date = time.ToString(format),
                    header = header,
                    isRead = false,
                    sender = "BiMaçVar!"
                });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}