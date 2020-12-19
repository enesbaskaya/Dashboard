﻿using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    public class PaymentController : Controller
    {
        private readonly Context _context;
        public PaymentController(Context context)
        {

            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var branchTransActionResult = await _context.branchTransActions.Include(x => x.card).Include(x => x.card.branch).ToListAsync();
            var depositTransActionResult = await _context.depositTransActions.Include(x => x.branch).ToListAsync();

            double sendData = await _context.branchTransActions.Where(x => x.checkActive).SumAsync(x => x.amount);
            double depositData = await _context.depositTransActions.Where(x => x.checkActive).SumAsync(x => x.amount);

            ViewBag.sendData = sendData;
            ViewBag.receiveData = depositData;
            ViewBag.profit = (depositData - sendData);

            ViewBag.branchTransActions = branchTransActionResult;
            ViewBag.depositTransActions = depositTransActionResult;
            return View();
        }


        public async Task<IActionResult> UpdateTransAction(long transId)
        {

            BranchTransActions updatedData = await _context.branchTransActions.Include(x => x.card).Include(x => x.card.branch).FirstOrDefaultAsync(x => x.transId == transId);
            if (!updatedData.checkActive)
            {
                updatedData.checkActive = true;
                _context.branchTransActions.Update(updatedData);

                Branch branch = updatedData.card.branch;

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
            DepositTransActions updatedData = await _context.depositTransActions.Include(x => x.branch).FirstOrDefaultAsync(x => x.transId == transId);
            if (!updatedData.checkActive)
            {
                updatedData.checkActive = true;
                _context.depositTransActions.Update(updatedData);

                Branch branch = updatedData.branch;

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

                BranchWallet wallet = await _context.branchWallet.FirstOrDefaultAsync(x => x.branchId == branch.branchId);
                wallet.debt -= amount;
                _context.branchWallet.Update(wallet);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteDepositTransAction(long transId)
        {
            DepositTransActions updatedData = await _context.depositTransActions.Include(x => x.branch).FirstOrDefaultAsync(x => x.transId == transId);
            _context.depositTransActions.Remove(updatedData);


            Branch branch = updatedData.branch;

            string paymentFailedReceiveContent = "Sayın, " + branch.admin + "\n'" + updatedData.transId + "' numaralı ödeme talimatınız iptal edilmiştir." +
                " Lütfen ödeme bilgilerini kontrol edip işlemi tekrar ediniz." +
                "\n\nSaygılarımızla, BiMaçVar!";

            string paymentSuccessHeader = "Ödeme Bildirimi";
            DateTime time = DateTime.Now;
            string format = "dd/M/yyyy";
            var insertNotification = await _context.branchNotifications.AddAsync(
                new BranchNotifications
                {
                    branchId = branch.branchId,
                    content = paymentFailedReceiveContent,
                    date = time.ToString(format),
                    header = paymentSuccessHeader,
                    isRead = false,
                    sender = "BiMaçVar!"
                });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
