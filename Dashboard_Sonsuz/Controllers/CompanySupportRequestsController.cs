using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public CompanySupportRequestsController(Context context)
        {

            _context = context;
        }

        [HttpPost]
        public ActionResult PartialViewEdit(long requestId)
        {
            CompanySupportRequests request = _context.companySupportRequests.Find(requestId);
            return PartialView("PartialViewEdit", request);
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
