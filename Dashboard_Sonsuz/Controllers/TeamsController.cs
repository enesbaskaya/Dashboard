using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{

    public class TeamsController : BaseController
    {
        public TeamsController(Context context, IConfiguration config) : base(context, config) { }

        public async Task<IActionResult> IndexAsync()
        {

            List<TeamData> teamData = await _context.teamData.Include(x => x.team).Include(x => x.team.user).Include(x => x.team.teamPlayers).ToListAsync();

            return View(teamData);
        }
    }
}
