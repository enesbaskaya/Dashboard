using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class UsersController : Controller
    {

        private readonly Context _context;
        private readonly IConfiguration _config;
        public UsersController(Context context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {

            List<User> users = _context.user
                .Include(x => x.status)
                .Include(x => x.contact)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .ToList();

            int waitingUsers = _context.user.Where(x => x.statusId == 1).Count();
            int activeUsers = _context.user.Where(x => x.statusId == 2).Count();
            int rejectedUsers = _context.user.Where(x => x.statusId == 3).Count();
            int deletedUsers = _context.user.Where(x => x.statusId == 4).Count();

            ViewBag.waitingUsers = waitingUsers;
            ViewBag.rejectedUsers = rejectedUsers;
            ViewBag.deletedUsers = deletedUsers;
            ViewBag.activeUsers = activeUsers;


            return View(users);
        }



        //modal sayfası partial view çekilecek.
        [HttpPost]
        public async Task<ActionResult> BanUserModal(long userId)
        {
            User user = await _context.user.FindAsync(userId);
            return PartialView("BanUserModal", user);
        }

        public async Task<IActionResult> BanUserAsync(long userId, string message)
        {
            User user = await _context.user
                .Include(x => x.contact)
                .FirstOrDefaultAsync(x => x.userId == userId);

            user.statusId = 4;
            _context.user.Update(user);
            await _context.SaveChangesAsync();

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "KULLANICI HESABINIZ YASAKLANDI";
            email.content = message;
            email.mail = user.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateUserAsync(long userId)
        {
            User user = await _context.user.FindAsync(userId);
            user.statusId = 2;
            _context.user.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




    }
}
