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
        private Admin admin;
        private readonly IConfiguration _config;
        public AreaApplicationsController(Context context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }

        public async Task<IActionResult> ApproveAreaAsync(long areaId)
        {
            AreaInfo area = await _context.areaInfo.FindAsync(areaId);
            area.isActive = true;
            _context.areaInfo.Update(area);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> IndexAsync()
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            List<AreaInfo> areas = _context.areaInfo.Where(x => !x.isActive).Include(x => x.branch).Include(x => x.branch.contact).ToList();

            return View(areas);
        }
    }
}
