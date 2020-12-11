using Dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> IndexAsync(int page = 1, int pageSize = 15)
        {
            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;

            var branchTransActionResult = (from a in _context.branchTransActions
                                           join c in _context.branchCards on a.cardId equals c.cardId
                                           join b in _context.branch on c.branchId equals b.branchId
                                           select new BranchTransActions
                                           {
                                               transId = a.transId,
                                               cardId = a.cardId,
                                               date = a.date,
                                               amount = a.amount,
                                               checkActive = a.checkActive,
                                               card = new BranchCards { cardId = c.cardId, iban = c.iban, cardOwner = c.cardOwner, bankName = c.bankName, branchId = c.branchId, branch = new Branch { name = b.name } }
                                           }).OrderByDescending(x => x.transId);

            PagedList<BranchTransActions> branchTransActionModel = new PagedList<BranchTransActions>(branchTransActionResult, page, pageSize);


            var depositTransActionResult = (from d in _context.depositTransActions
                                            join b in _context.branch on d.branchId equals b.branchId
                                            select new DepositTransActions
                                            {
                                                transId = d.transId,
                                                date = d.date,
                                                amount = d.amount,
                                                checkActive = d.checkActive,
                                                branchId = d.branchId,
                                                branch = new Branch { name = b.name, admin = b.admin }
                                            }).OrderByDescending(x => x.transId);

            PagedList<DepositTransActions> depositTransActionModel = new PagedList<DepositTransActions>(depositTransActionResult, page, pageSize);


            double sendData = await _context.branchTransActions.Where(x => x.amount > 0).SumAsync(x => x.amount);
            double depositData = (await _context.depositTransActions.Where(x => x.amount > 0).SumAsync(x => x.amount));

            ViewBag.sendData = sendData;
            ViewBag.receiveData = depositData;
            ViewBag.profit = (depositData - sendData);

            ViewBag.branchTransActions = branchTransActionModel;
            ViewBag.depositTransActions = depositTransActionModel;
            return View();
        }


        public async Task<IActionResult> UpdateTransAction(long transId)
        {
            BranchTransActions updatedData = await _context.branchTransActions.FindAsync(transId);
            if (!updatedData.checkActive)
            {
                updatedData.checkActive = true;
                _context.branchTransActions.Update(updatedData);

                var result = (from a in _context.branchTransActions
                              join c in _context.branchCards on a.cardId equals c.cardId
                              join b in _context.branch on c.branchId equals b.branchId
                              where a.transId == transId
                              select new Branch
                              {
                                  branchId = b.branchId,
                                  admin = b.admin,
                              }).ToList();
                Branch branch = result[0];

                string paymentSuccessSendContent = "Sayın, " + branch.admin + "\n'" + updatedData.transId + "' numaralı ödeme talimatınız başarılı bir şekilde " +
                    " belirttiğiniz banka hesabınıza aktarılmıştır. Cüzdan sayfanızdan gerekli takibatı yapabilirsiniz.\n\nSaygılarımızla, BiMaçVar!";



                string paymentSuccessHeader = "Ödeme Bildirimi";
                DateTime time = DateTime.Now;
                string format = "dd/M/yyyy";
                var insertNotification = await _context.branchNotifications.AddAsync(
                    new BranchNotifications
                    {
                        branchId = branch.branchId,
                        content = paymentSuccessSendContent,
                        date = time.ToString(format),
                        header = paymentSuccessHeader,
                        isRead = false,
                        sender = "BiMaçVar!"
                    });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateDepositTransAction(long transId, double amount)
        {
            DepositTransActions updatedData = await _context.depositTransActions.FindAsync(transId);
            if (!updatedData.checkActive)
            {
                updatedData.checkActive = true;
                _context.depositTransActions.Update(updatedData);

                var result = (from a in _context.depositTransActions
                              join b in _context.branch on a.branchId equals b.branchId
                              where a.transId == transId
                              select new Branch
                              {
                                  branchId = b.branchId,
                                  admin = b.admin,
                              }).ToList();
                Branch branch = result[0];

                string paymentSuccesReceiveContent = "Sayın, " + branch.admin + "\n'" + updatedData.transId + "' numaralı ödeme talimatınız başarıyla gerçekleştirildi." +
                    " Cüzdan sayfanızdan gerekli takibatı yapabilirsiniz.\n\nSaygılarımızla, BiMaçVar!";
                string paymentSuccessHeader = "Ödeme Bildirimi";
                DateTime time = DateTime.Now;
                string format = "dd/M/yyyy";
                var insertNotification = await _context.branchNotifications.AddAsync(
                    new BranchNotifications
                    {
                        branchId = branch.branchId,
                        content = paymentSuccesReceiveContent,
                        date = time.ToString(format),
                        header = paymentSuccessHeader,
                        isRead = false,
                        sender = "BiMaçVar!"
                    });

                var debtResult = (from a in _context.branchWallet
                                  join b in _context.depositTransActions on a.branchId equals b.branchId
                                  where b.transId == transId
                                  select new BranchWallet
                                  {
                                      debt = a.debt,
                                      balance = a.balance,
                                      branchId = a.branchId,
                                      walletId = a.walletId
                                  }).ToList();

                BranchWallet wallet = debtResult[0];
                wallet.debt -= amount;
                _context.branchWallet.Update(wallet);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteDepositTransAction(long transId)
        {
            DepositTransActions updatedData = await _context.depositTransActions.FindAsync(transId);
            if (!updatedData.checkActive)
            {
                updatedData.checkActive = true;
                _context.depositTransActions.Update(updatedData);

                var result = (from a in _context.depositTransActions
                              join b in _context.branch on a.branchId equals b.branchId
                              select new Branch
                              {
                                  branchId = b.branchId,
                                  admin = b.admin,
                              }).ToList();
                Branch branch = result[0];

                string paymentSuccesReceiveContent = "Sayın, " + branch.admin + "\n'" + updatedData.transId + "' numaralı ödeme talimatınız başarıyla gerçekleştirildi." +
                    " Cüzdan sayfanızdan gerekli takibatı yapabilirsiniz.\n\nSaygılarımızla, BiMaçVar!";

                string paymentSuccessHeader = "Ödeme Bildirimi";
                DateTime time = DateTime.Now;
                string format = "dd/M/yyyy";
                var insertNotification = await _context.branchNotifications.AddAsync(
                    new BranchNotifications
                    {
                        branchId = branch.branchId,
                        content = paymentSuccesReceiveContent,
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
