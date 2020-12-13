using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using System;

namespace Dashboard.Controllers
{
    public class BranchesController : Controller
    {
        private Admin admin;
        private readonly IConfiguration _config;
        private readonly Context _context;

        public BranchesController(Context context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IActionResult> IndexAsync()
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            List<Branch> activeBranches = (from b in _context.branch
                                           join c in _context.contactInfo on b.contactId equals c.contactId
                                           join d in _context.districts on c.districtId equals d.districtId
                                           join ct in _context.city on d.cityId equals ct.cityId
                                           where b.isActive
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
                                           }).ToList();

            int waitingBranches = _context.branch.Where(x => !x.isActive).Count();

            ViewBag.waitingBranches = waitingBranches;

            return View(activeBranches);
        }

        public async Task<IActionResult> DeleteBranch(long branchId)
        {

            Branch updatedData = await _context.branch.FindAsync(branchId);
            var contactInfo = _context.contactInfo.FirstOrDefault(x => x.contactId == updatedData.contactId);
            _context.branch.Remove(updatedData);
            await _context.SaveChangesAsync();

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetValue<String>("SMTP:Username")));
            email.To.Add(MailboxAddress.Parse(contactInfo.mail));
            email.Subject = "Hesap Silinmesi - BiMaçVar!";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = "Sevgili " + updatedData.admin + ",\nHesabınız silinmiştir. Eğer bir yanlışlık olduğunu" +
                " düşünüyorsanız support@bimacvar.com adresinden ya da diğer iletişim araçlarını kullanarak bize ulaşabilirsiniz."
            };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetValue<String>("SMTP:Host"), _config.GetValue<int>("SMTP:Port"), false);
            smtp.Authenticate(_config.GetValue<String>("SMTP:Username"), _config.GetValue<String>("SMTP:Password"));
            smtp.Send(email);
            smtp.Disconnect(true);


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeactiveBranch(long branchId)
        {

            Branch updatedData = await _context.branch.FindAsync(branchId);
            updatedData.isActive = false;
            _context.branch.Update(updatedData);

            var contactInfo = _context.contactInfo.FirstOrDefault(x => x.contactId == updatedData.contactId);



            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetValue<String>("SMTP:Username")));
            email.To.Add(MailboxAddress.Parse(contactInfo.mail));
            email.Subject = "Hesap Deaktivasyonu - BiMaçVar!";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = "Sevgili " + updatedData.admin + ",\nHesabınız pasif hale getirilmiştir. Eğer bir yanlışlık olduğunu" +
                " düşünüyorsanız support@bimacvar.com adresinden ya da diğer iletişim araçlarını kullanarak bize ulaşabilirsiniz."
            };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetValue<String>("SMTP:Host"), _config.GetValue<int>("SMTP:Port"), false);
            smtp.Authenticate(_config.GetValue<String>("SMTP:Username"), _config.GetValue<String>("SMTP:Password"));
            smtp.Send(email);
            smtp.Disconnect(true);



            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
