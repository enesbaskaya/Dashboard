using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
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


            double transSendData = await _context.transActions.Where(x => x.amount > 0).SumAsync(x => x.amount);
            double transReceiveData = -(await _context.transActions.Where(x => x.amount < 0).SumAsync(x => x.amount));

            ViewBag.sendData = transSendData;
            ViewBag.receiveData = transReceiveData;
            ViewBag.worth = transReceiveData - transSendData;


            return View(model);
        }


        public async Task<IActionResult> Update(long transId)
        {
            TransActions updatedData = await _context.transActions.FindAsync(transId);
            if (!updatedData.checkActive)
            {
                updatedData.checkActive = true;
                _context.transActions.Update(updatedData);

                var result = (from a in _context.transActions
                              join c in _context.branchCards on a.cardId equals c.cardId
                              join b in _context.branch on c.branchId equals b.branchId
                              select new Branch
                              {
                                  branchId = b.branchId,
                                  admin = b.admin,
                              }).ToList();
                Branch branch = result[0];

                //true = sender is BMV else sender is Branch.
                bool moneySender = (updatedData.amount > 0);

                string paymentSuccessSendContent = "Sayın, " + branch.admin + "\n'" + updatedData.transId + "' numaralı ödeme talimatınız başarılı bir şekilde " +
                    " belirttiğiniz banka hesabınıza aktarılmıştır. Cüzdan sayfanızdan gerekli takibatı yapabilirsiniz.\n\nSaygılarımızla, BiMaçVar!";

                string paymentSuccesReceiveContent = "Sayın, " + branch.admin + "\n'" + updatedData.transId + "' numaralı ödeme gerçekleşti ve para başarılı bir şekilde hesabımıza ulaştı." +
                    " Cüzdan sayfanızdan gerekli takibatı yapabilirsiniz.\n\nSaygılarımızla, BiMaçVar!";

                string paymentSuccessHeader = "Ödeme Bildirimi";
                DateTime time = DateTime.Now;
                string format = "dd/M/yyyy";
                var insertNotification = await _context.branchNotifications.AddAsync(
                    new BranchNotifications
                    {
                        branchId = branch.branchId,
                        content = moneySender ? paymentSuccessSendContent : paymentSuccesReceiveContent,
                        date = time.ToString(format),
                        header = paymentSuccessHeader,
                        isRead = false,
                        sender = "BiMaçVar!"
                    });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
