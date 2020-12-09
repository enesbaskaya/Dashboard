using Dashboard_Sonsuz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class PaymentController : Controller
    {

        private Admin admin;
        private readonly Context _context;

        public PaymentController(Context context)
        {

            _context = context;
        }
        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 30)
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            var result = (from a in _context.transActions
                          join c in _context.branchCards on a.cardId equals c.cardId
                          select new TransActions
                          {
                              transId = a.transId,
                              cardId = a.cardId,
                              date = a.date,
                              amount = a.amount,
                              checkActive = a.checkActive,
                              card = new BranchCards(c.cardId, c.iban, c.cardOwner, c.bankName, c.branchId)
                          }).OrderByDescending(x => x.transId);

            PagedList<TransActions> model = new PagedList<TransActions>(result, page, pageSize);

            return View(model);
        }


        public async Task<IActionResult> Update(long transId)
        {
            TransActions updatedData = await _context.transActions.FindAsync(transId);
            updatedData.checkActive = true;
            _context.transActions.Update(updatedData);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
