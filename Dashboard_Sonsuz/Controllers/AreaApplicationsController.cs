using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class AreaApplicationsController : Controller
    {

        private readonly Context _context;
        private readonly IConfiguration _config;
        public AreaApplicationsController(Context context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }

        public async Task<IActionResult> ApproveAreaAsync(long areaId)
        {
            AreaInfo area = await _context.areaInfo
                .Include(x => x.branch)
                .Include(x => x.branch.contact)
                .FirstOrDefaultAsync(x => x.areaId == areaId);
            area.statusId = 2;
            _context.areaInfo.Update(area);
            await _context.SaveChangesAsync();

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! Sahanız Onaylandı";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletmenize ait " + area.areaName + " " +
                "isimli sahanız onaylanmıştır. Sahalarım sayfasından sahanızla ilgili tüm seçenekleri görebilir ve düzenleyebilirsiniz." +
                " Ayrıca saha fotoğraflarını da o alandan ekleyebilirsiniz. Artık siz de BiMaçVar! ile kazancınıza kazanç katacaksınız!";
            email.mail = area.branch.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> DeleteAreaModalAsync(long areaId)
        {
            AreaInfo areaInfo = await _context.areaInfo.Include(x => x.branch).Include(x => x.branch.contact).FirstOrDefaultAsync(x => x.areaId == areaId);
            return PartialView("DeleteAreaModal", areaInfo);
        }

        public async Task<IActionResult> DeleteAreaAsync(long areaId, string message)
        {

            AreaInfo deletedData = await _context.areaInfo
                .Include(x => x.branch)
                .Include(x => x.branch.contact)
                .FirstOrDefaultAsync(x => x.areaId == areaId);
            deletedData.statusId = 4;
            _context.areaInfo.Update(deletedData);
            await _context.SaveChangesAsync();


            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! Sahanız Silindi";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletmenize ait " + deletedData.areaName + " isimli sahanız silinmiştir. Gerekçe;\n"
                + message +
                " Bir yanlışlık olduğunu" +
                " düşünüyorsanız ya da itirazda bulunmak istiyorsanız bize destek talebinde bulunabilirsiniz.";
            email.mail = deletedData.branch.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index", new { branchId = deletedData.branchId });
        }


        [HttpPost]
        public async Task<ActionResult> RejectAreaModalAsync(long areaId)
        {
            AreaInfo areaInfo = await _context.areaInfo.Include(x => x.branch).Include(x => x.branch.contact).FirstOrDefaultAsync(x => x.areaId == areaId);
            return PartialView("RejectAreaModal", areaInfo);
        }

        public async Task<IActionResult> RejectAreaAsync(long areaId, string message)
        {
            AreaInfo area = await _context.areaInfo
            .Include(x => x.branch)
            .Include(x => x.branch.contact)
            .FirstOrDefaultAsync(x => x.areaId == areaId);
            area.statusId = 3;
            _context.areaInfo.Update(area);
            await _context.SaveChangesAsync();

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! Sahanız Onaylandı";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletmenize ait " + area.areaName + " " +
                "isimli sahanız onay aşamasını geçememiştir. Gerekçe:\n" + message + "\n\n" +
                "Lütfen tüm kontrolleri sağladıktan sonra tekrar saha başvurusunda bulununuz.";
            email.mail = area.branch.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {

            List<AreaInfo> areas = _context.areaInfo
                 .Where(x => x.statusId == 1)
                 .Include(x => x.status)
                 .Include(x => x.branch)
                 .Include(x => x.branch.contact)
                 .ToList();

            return View(areas);
        }
    }
}
