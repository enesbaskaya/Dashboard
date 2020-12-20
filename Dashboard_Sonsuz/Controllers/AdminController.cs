using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Dashboard.Controllers
{
    public class AdminController : BaseController
    {

        public AdminController(Context context, IConfiguration config) : base(context, config) { }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string inputMail, string inputPassword)
        {
            Admin admin = _context.admin.FirstOrDefault(x => x.username == inputMail && x.password == inputPassword);
            if (admin != null)
            {
                Program.administrator = admin;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["alertMessage"] = "Böyle bir kullanıcı bulunamadı!";
            }


            return View();
        }

        public IActionResult Logout()
        {
            Program.administrator = null;
            return RedirectToAction("Index", "Admin");
        }
    }
}
