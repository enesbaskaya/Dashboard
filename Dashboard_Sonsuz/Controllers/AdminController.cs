using Dashboard.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly Context _context;

        public AdminController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string inputMail, string inputPassword)
        {
            var bilgiler = _context.admin.FirstOrDefault(x => x.username == inputMail && x.password == inputPassword);
            if (bilgiler != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, bilgiler.username)
                };
                var useridentity = new ClaimsIdentity(claims, "Home");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                HttpContext.Session.SetString("admin", bilgiler.username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["alertMessage"] = "Böyle bir kullanıcı bulunamadı!";
            }


            return View();
        }
    }
}
