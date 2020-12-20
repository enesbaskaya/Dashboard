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


        [HttpPost]
        public async Task<ActionResult> DeleteBranchModalAsync(long branchId)
        {
            Branch card = await _context.branch.FindAsync(branchId);
            return PartialView("DeleteBranchModal", card);
        }

        public async Task<IActionResult> DeleteBranchAsync(long branchId, string message)
        {

            Branch branch = await _context.branch.Include(x => x.contact).FirstOrDefaultAsync(x => x.branchId == branchId);
            branch.statusId = 4;
            _context.branch.Update(branch);
            

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! İşletme Hesabınız Devredışı Bırakıldı";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletme hesabınız yasaklanmıştır. Sebep;\n" + message + "\n\nEğer bir" +
                " yanlışlık olduğunu düşünüyorsanız lütfen bizlerle iletişime geçiniz.";
            email.mail = branch.contact.mail;
            await email.sendAsync(_config);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeactiveBranchModalAsync(long branchId)
        {
            Branch card = await _context.branch.FindAsync(branchId);
            return PartialView("DeactiveBranchModal", card);
        }

        public async Task<IActionResult> DeactiveBranchAsync(long branchId, string message)
        {

            Branch branch = await _context.branch.Include(x => x.contact).FirstOrDefaultAsync(x => x.branchId == branchId);
            branch.statusId = 1;
            _context.branch.Update(branch);

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! İşletme Hesabınız Devredışı Bırakıldı";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletme hesabınız deaktif duruma getirilmiştir. Sebep;\n" + message + "\n\nEğer bir" +
                " yanlışlık olduğunu düşünüyorsanız lütfen bizlerle iletişime geçiniz.";
            email.mail = branch.contact.mail;
            await email.sendAsync(_config);



            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
