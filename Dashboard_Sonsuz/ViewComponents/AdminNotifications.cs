using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.ViewComponents
{
    public class AdminNotifications : ViewComponent
    {

        private readonly Context _context;
        public AdminNotifications(Context context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(long notificationId)
        {
            List<AdminNotification> adminNotifications = _context.adminNotification.Where(x => !x.active)
                .Include(x => x.notificationType)
                .OrderByDescending(x => x.adminNotificationId)
                .ToList();
            return View(adminNotifications);
        }



    }
}
