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

namespace Dashboard.Controllers
{
    public class BranchProfileController : Controller
    {

        LocServices locServices;
        private readonly Context _context;
        private Admin admin;

        Branch branch;

        public BranchProfileController(LocServices services, Context context)
        {
            this.locServices = services;
            this._context = context;
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

        public async Task<IActionResult> IndexAsync(long branchId)
        {
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


    }
}
