using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Dashboard.Controllers
{
    public class BranchApplicationController : BaseController
    {

        public BranchApplicationController(Context context, IConfiguration config) : base(context, config) { }

        public IActionResult Index()
        {
            List<Branch> deActiveBranches = _context.branch
                .Where(x => x.statusId == 1)
                .Include(x => x.status)
                .Include(x => x.contact)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .ToList();
            return View(deActiveBranches);

        }


        public async Task<IActionResult> ApproveBranch(long branchId)
        {

            Branch updatedData = await _context.branch.FindAsync(branchId);

            updatedData.statusId = 2;
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


        [HttpPost]
        public async Task<ActionResult> RejectBranchModalAsync(long branchId)
        {
            Branch branch = await _context.branch.Include(x => x.contact).FirstOrDefaultAsync(x => x.branchId == branchId);
            return PartialView("RejectBranchModal", branch);
        }

        public async Task<IActionResult> RejectBranchAsync(long branchId, string message)
        {
            Branch branch = await _context.branch.Include(x => x.contact).FirstOrDefaultAsync(x => x.branchId == branchId);
            branch.statusId = 3;
            _context.branch.Update(branch);
            await _context.SaveChangesAsync();

            var email = new SMTPMail();
            email.mailType = MailTypes.INFORMATION;
            email.header = "SAHA BAŞVURUNUZ REDDEDİLDİ";
            email.content = message;
            email.mail = branch.contact.mail;
            await email.sendAsync(_config);


            return RedirectToAction("Index");
        }


    }
}
