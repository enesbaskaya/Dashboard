﻿using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Dashboard.Controllers
{
    public class BranchProfileController : BaseController
    {

        private LocServices locServices;
        private Branch branch;

        public BranchProfileController(LocServices locServices, Context context, IConfiguration config) : base(context, config) { this.locServices = locServices; }



        public List<String> openDays()
        {

            List<String> days = new List<String>() { locServices.GetLocalizedHtmlString("Sunday"),
                locServices.GetLocalizedHtmlString("Monday"),
                locServices.GetLocalizedHtmlString("Tuesday"),
                locServices.GetLocalizedHtmlString("Wednesday"),
                locServices.GetLocalizedHtmlString("Thursday"),
                locServices.GetLocalizedHtmlString("Friday"),
                locServices.GetLocalizedHtmlString("Saturday"),
            };
            List<String> openDays = new List<String>();
            int i = 0;
            foreach (String s in branch.openDays.Split("-"))
            {
                bool open = bool.Parse(s);
                if (open)
                {
                    openDays.Add(days[i]);
                }
                i++;
            }

            return openDays;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBranchAsync(
            string branchOwner,
            string branchName,
            string taxNumber,
            string telephone,
            string address,
            string mail,
            string debt,
            string balance,
            bool shoesRent,
            bool market,
            bool catering,
            bool shower,
            long branchId
            )
        {

            Branch b = await _context.branch
                .Include(x => x.contact)
                .Include(x => x.status)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .Include(x => x.contact.district.city.region).FirstOrDefaultAsync(x => x.branchId == branchId);
            BranchWallet wallet = await _context.branchWallet.FirstOrDefaultAsync(x => x.branchId == branchId);

            b.admin = branchOwner;
            b.name = branchName;
            b.taxNumber = taxNumber;
            b.contact.address = address;
            b.contact.telephone = telephone;
            b.contact.mail = mail;
            b.shoesRent = shoesRent;
            b.market = market;
            b.catering = catering;
            b.shower = shower;
            wallet.debt = double.Parse(debt);
            wallet.balance = double.Parse(balance);


            List<BranchPaymentMethods> methods = _context.branchPaymentMethods.Where(x => x.branchId == branchId).Include(x => x.paymentMethod).ToList();


            _context.branch.Update(b);
            _context.contactInfo.Update(b.contact);
            _context.branchWallet.Update(wallet);

            _context.SaveChanges();

            return RedirectToAction("Index", new { branchId = branchId });
        }

        public async Task<IActionResult> IndexAsync(long branchId)
        {
            this.branch = await _context.branch
                .Include(x => x.contact)
                .Include(x => x.status)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .Include(x => x.contact.district.city.region).FirstOrDefaultAsync(x => x.branchId == branchId);

            BranchWallet wallet = await _context.branchWallet.FirstOrDefaultAsync(x => x.branchId == branchId);
            BranchStars stars = await _context.branchStars.FirstOrDefaultAsync(x => x.branchId == branchId);

            List<BranchPaymentMethods> branchPaymentMethods = await _context.branchPaymentMethods.Where(x => x.branchId == branchId).Include(x => x.paymentMethod).ToListAsync();
            List<PaymentMethods> paymentMethods = await _context.paymentMethods.ToListAsync();


            List<BranchCards> branchCards = _context.branchCards.Where(x => x.branchId == branchId).ToList();
            List<BranchEconomy> branchEconomies = _context.branchEconomy.Where(x => x.branchId == branchId).ToList();
            List<MatchHistory> matchHistories = _context.matchHistory.
                Where(x => x.area.branchId == branchId)
                .Include(x => x.area)
                .Include(x => x.loserTeam)
                .Include(x => x.winnerTeam)
                .Include(x => x.appointmentType)
                .Include(x => x.paymentMethod)
                .ToList();

            List<BranchTransActions> branchTransActions = _context.branchTransActions.
                Where(x => x.card.branchId == branchId)
                .Include(x => x.status)
                .Include(x => x.card)
                .ToList();

            List<AreaInfo> areas = _context.areaInfo.Where(x => x.branchId == branchId).Include(x => x.status).ToList();

            ViewBag.walletData = wallet;
            ViewBag.branch = branch;
            ViewBag.branchStars = stars;

            ViewBag.branchCards = branchCards;
            ViewBag.matchHistories = matchHistories;
            ViewBag.branchEconomies = branchEconomies;
            ViewBag.branchTransActions = branchTransActions;
            ViewBag.paymentMethods = paymentMethods;
            ViewBag.branchPaymentMethods = branchPaymentMethods;
            ViewBag.areas = areas;

            return View();
        }

        [HttpPost]
        public JsonResult GetCityDistrictData(int? ilID, string tip)
        {
            //geriye döndüreceğim sonucListim
            List<SelectListItem> sonuc = new List<SelectListItem>();
            //bu işlem başarılı bir şekilde gerçekleşti mi onun kontrolunnü yapıyorum
            bool basariliMi = true;
            try
            {
                switch (tip)
                {
                    case "ilGetir":
                        //veritabanımızdaki iller tablomuzdan illerimizi sonuc değişkenimze atıyoruz
                        foreach (var il in _context.city.ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = il.cityName,
                                Value = il.cityId.ToString()
                            });
                        }
                        break;
                    case "ilceGetir":
                        //ilcelerimizi getireceğiz ilimizi selecten seçilen ilID sine göre 
                        foreach (var ilce in _context.districts.Where(il => il.cityId == ilID).ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = ilce.districtName,
                                Value = ilce.districtId.ToString()
                            });
                        }
                        break;

                    default:
                        break;

                }
            }
            catch (Exception)
            {
                //hata ile karşılaşırsak buraya düşüyor
                basariliMi = false;
                sonuc = new List<SelectListItem>();
                sonuc.Add(new SelectListItem
                {
                    Text = "Bir hata oluştu :(",
                    Value = "Default"
                });

            }
            //Oluşturduğum sonucları json olarak geriye gönderiyorum
            return Json(new { ok = basariliMi, text = sonuc });
        }
        public async Task<IActionResult> DeleteCard(long branchId, long cardId)
        {

            BranchCards deletedData = await _context.branchCards.FindAsync(cardId);
            _context.branchCards.Remove(deletedData);
            await _context.SaveChangesAsync();

            Branch b = await _context.branch.Include(x => x.contact).FirstOrDefaultAsync(x => x.branchId == branchId);


            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "Kart Silinmesi - BiMaçVar!";
            email.content = "Sevgili " + b.admin + ",\n'" + deletedData.iban + "' numaralı banka hesabınız hesabınızdan kaldırılmıştır. Eğer bir yanlışlık olduğunu" +
                " düşünüyorsanız support@bimacvar.com adresinden ya da diğer iletişim araçlarını kullanarak bize ulaşabilirsiniz.";
            email.mail = b.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index", new { branchId = branchId });
        }

        [HttpPost]
        public async Task<ActionResult> EditCardModalAsync(long cardId)
        {
            BranchCards card = await _context.branchCards.FindAsync(cardId);
            return PartialView("EditCardModal", card);
        }

        public async Task<IActionResult> EditCardAsync(long cardId, string bankName, string cardOwner, string IBAN)
        {
            BranchCards card = await _context.branchCards.FindAsync(cardId);
            card.iban = IBAN;
            card.cardOwner = cardOwner;
            card.bankName = bankName;
            _context.branchCards.Update(card);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { branchId = card.branchId });
        }



        public async Task<IActionResult> ApproveAreaAsync(long areaId)
        {
            AreaInfo area = await _context.areaInfo
                .Include(x => x.branch)
                .Include(x => x.branch.contact)
                .FirstOrDefaultAsync(x => x.areaId == areaId);
            area.statusId = 2;
            _context.areaInfo.Update(area);
            await _context.SaveChangesAsync();

            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! Sahanız Onaylandı";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletmenize ait " + area.areaName + " " +
                "isimli sahanız onaylanmıştır. Sahalarım sayfasından sahanızla ilgili tüm seçenekleri görebilir ve düzenleyebilirsiniz." +
                " Ayrıca saha fotoğraflarını da o alandan ekleyebilirsiniz. Artık siz de BiMaçVar! ile kazancınıza kazanç katacaksınız!";
            email.mail = area.branch.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index", new { branchId = area.branchId });
        }


        [HttpPost]
        public async Task<ActionResult> DeleteAreaModalAsync(long areaId)
        {
            AreaInfo areaInfo = await _context.areaInfo.Include(x => x.branch).Include(x => x.branch.contact).FirstOrDefaultAsync(x => x.areaId == areaId);
            return PartialView("DeleteAreaModal", areaInfo);
        }


        [HttpGet]
        public async Task<ActionResult> DeletePaymentMethodAsync(long branchPaymentMethodId)
        {
            BranchPaymentMethods bpm = await _context.branchPaymentMethods.FindAsync(branchPaymentMethodId);
            _context.branchPaymentMethods.Remove(bpm);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { branchId = bpm.branchId });
        }

        [HttpPost]
        public async Task<ActionResult> AddPaymentMethodAsync(long branchId, long? paymentMethodId)
        {
            if (paymentMethodId != null)
            {
                BranchPaymentMethods bpm = new BranchPaymentMethods { branchId = branchId, paymentMethodId = paymentMethodId.Value };
                await _context.branchPaymentMethods.AddAsync(bpm);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { branchId = branchId });
        }

        public async Task<IActionResult> DeleteAreaAsync(long areaId, string message)
        {

            AreaInfo area = await _context.areaInfo
                .Include(x => x.branch)
                .Include(x => x.branch.contact)
                .FirstOrDefaultAsync(x => x.areaId == areaId);
            area.statusId = 4;
            _context.areaInfo.Update(area);
            await _context.SaveChangesAsync();


            var email = new SMTPMail();
            email.mailType = MailTypes.WARNING;
            email.header = "BiMaçVar! Sahanız Silindi";
            email.content = "Değerli BiMaçVar! kullanıcısı, işletmenize ait " + area.areaName + " isimli sahanız silinmiştir. Gerekçe;\n"
                + message +
                " Bir yanlışlık olduğunu" +
                " düşünüyorsanız ya da itirazda bulunmak istiyorsanız bize destek talebinde bulunabilirsiniz.";
            email.mail = area.branch.contact.mail;
            await email.sendAsync(_config);

            return RedirectToAction("Index", new { branchId = area.branchId });
        }

    }
}
