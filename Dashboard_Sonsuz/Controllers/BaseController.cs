using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly Context _context;
        protected readonly IConfiguration _config;

        public BaseController(Context context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }

    }
}
