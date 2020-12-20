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
    public class MatchHistoryController : BaseController
    {


        public MatchHistoryController(Context context, IConfiguration config) : base(context, config) { }

        public IActionResult Index()
        {
            List<MatchHistory> historyMatches = _context.matchHistory
                .Include(x => x.area)
                .Include(x => x.area.branch)
                .Include(x => x.loserTeam)
                .Include(x => x.winnerTeam)
                .Include(x => x.winnerTeam.user)
                .Include(x => x.loserTeam.user)
                .Include(x => x.appointmentType)
                .Include(x => x.paymentMethod)
                .ToList();
            ViewBag.sendData = _context.matchHistory.Count();


            ViewBag.historyMatches = historyMatches;
            return View();
        }
    }
}
