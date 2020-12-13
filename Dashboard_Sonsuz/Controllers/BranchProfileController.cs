using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace Dashboard.Controllers
{
    public class BranchProfileController : Controller
    {

        LocServices locServices;
        private readonly Context _context;
        private Admin admin;
        private readonly IConfiguration _config;

        private Branch branch;

        public BranchProfileController(LocServices services, Context context, IConfiguration config)
        {
            this.locServices = services;
            this._context = context;
            this._config = config;
        }



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
        public IActionResult UpdateBranch(string branchOwner,
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

            Branch b = _context.branch.Where(x => x.branchId == branchId)
                .Include(x => x.contact)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .Include(x => x.contact.district.city.region).ToList()[0];

            BranchWallet wallet = _context.branchWallet.Where(x => x.branchId == branchId).ToList()[0];

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

            _context.branch.Update(b);
            _context.contactInfo.Update(b.contact);
            _context.branchWallet.Update(wallet);

            _context.SaveChanges();


            return RedirectToAction("Index", new { branchId = branchId });
        }

        public async Task<IActionResult> IndexAsync(long branchId)
        {
            Debug.Write("Girdi: " + branchId.ToString());

            this.admin = await _context.admin.FirstOrDefaultAsync(x => x.username == HttpContext.Session.GetString("admin"));
            ViewBag.admin = this.admin;
            DateTime time = DateTime.Now;
            this.branch = _context.branch.Where(x => x.branchId == branchId)
                .Include(x => x.contact)
                .Include(x => x.contact.district)
                .Include(x => x.contact.district.city)
                .Include(x => x.contact.district.city.region).ToList()[0];
            BranchWallet wallet = await _context.branchWallet.FindAsync(branchId);
            BranchStars stars = _context.branchStars.Where(x => x.branchId == branchId).Where(x => x.year == time.Year.ToString()).ToList()[0];


            List<BranchCards> branchCards = _context.branchCards.Where(x => x.branchId == branchId).ToList();
            List<BranchEconomy> branchEconomies = _context.branchEconomy.Where(x => x.branchId == branchId).ToList();
            List<BranchMatchWorth> branchMatchWorths = _context.branchMatchWorth.
                Where(x => x.match.area.branchId == branchId)
                .Include(x => x.match)
                .Include(x => x.match.area)
                .Include(x => x.match.loserTeam)
                .Include(x => x.match.winnerTeam)
                .ToList();
            List<BranchTransActions> branchTransActions = _context.branchTransActions.
                Where(x => x.card.branchId == branchId)
                .Include(x => x.card)
                .ToList();

            List<Regions> allRegions = _context.regions.ToList();
            List<City> allCities = _context.city.ToList();
            List<Districts> allDistricts = _context.districts.ToList();



            ViewBag.walletData = wallet;
            ViewBag.branch = branch;
            ViewBag.branchStars = stars;

            ViewBag.branchCards = branchCards;
            ViewBag.matchWorths = branchMatchWorths;
            ViewBag.branchEconomies = branchEconomies;
            ViewBag.branchTransActions = branchTransActions;

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

            Branch b = _context.branch.Where(x => x.branchId == branchId).Include(x => x.contact).ToList()[0];

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetValue<String>("SMTP:Username")));
            email.To.Add(MailboxAddress.Parse(b.contact.mail));
            email.Subject = "Kart Silinmesi - BiMaçVar!";
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = "Sevgili " + b.admin + ",\n'" + deletedData.iban + "' numaralı banka hesabınız hesabınızdan kaldırılmıştır. Eğer bir yanlışlık olduğunu" +
                " düşünüyorsanız support@bimacvar.com adresinden ya da diğer iletişim araçlarını kullanarak bize ulaşabilirsiniz."
            };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetValue<String>("SMTP:Host"), _config.GetValue<int>("SMTP:Port"), false);
            smtp.Authenticate(_config.GetValue<String>("SMTP:Username"), _config.GetValue<String>("SMTP:Password"));
            smtp.Send(email);
            smtp.Disconnect(true);


            return RedirectToAction("Index", new { branchId = branchId });
        }

    }
}
