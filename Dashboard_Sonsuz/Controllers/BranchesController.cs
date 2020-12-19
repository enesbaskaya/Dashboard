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
        private readonly IConfiguration _config;
        private readonly Context _context;

        public BranchesController(Context context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            List<Branch> branches = _context.branch
                //.Where(x => x.statusId != 1)
                .Include(x => x.contact)
                .Include(x => x.status)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .ToList();

            int waitingBranches = _context.branch.Where(x => x.statusId == 1).Count();
            int activeBranches = _context.branch.Where(x => x.statusId == 2).Count();
            int rejectedBranches = _context.branch.Where(x => x.statusId == 3).Count();
            int deletedBranches = _context.branch.Where(x => x.statusId == 4).Count();


            ViewBag.waitingBranches = waitingBranches;
            ViewBag.deletedBranches = deletedBranches;
             ViewBag.activeBranches = activeBranches;
            ViewBag.rejectedBranches = rejectedBranches;

            return View(branches);
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
            updatedData.statusId = 1;
            _context.branch.Update(updatedData);

            var contactInfo = _context.contactInfo.FirstOrDefault(x => x.contactId == updatedData.contactId);

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! İşletme Hesabınız Devredışı Bırakıldı";
            email.content = "Değerli BiMaçVar! kullanıcısı, hesabınız gayrinizami ölçütleri kullanmanız hasebiyle devredışı bırakılmıştır!" +
                " Eğer bir sorun olduğunu düşünüyorsanız lütfen iletişim adreslerimiz ile bizlerle bağlantı kurunuz!";
            email.mail = contactInfo.mail;
            await email.sendAsync(_config);



            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
