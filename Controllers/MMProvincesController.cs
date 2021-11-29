using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MMClubs.Models;

namespace MMClubs.Controllers
{
    public class MMProvincesController : Controller
    {
        private readonly MMClubsContext _context;

        public MMProvincesController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMProvinces
        public async Task<IActionResult> Index(string CountryCode)
        {
            if (!string.IsNullOrEmpty(CountryCode))
            {
                //store in cookie or session
                Response.Cookies.Append("", CountryCode);
                HttpContext.Session.SetString("CountryCode", CountryCode);

            }
            else if (Request.Query["CountryCode"].Any())
            {
                CountryCode = Request.Query["CountryCode"].ToString();
                //store in cookie or session
                Response.Cookies.Append("", CountryCode);
                HttpContext.Session.SetString("CountryCode", CountryCode);
            }
            else if(Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                CountryCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                CountryCode = HttpContext.Session.GetString("CountryCode");
            }
            else
            {
                //Redirecting back to Country with the message
                TempData["message"] = "Please select a Country";
                return RedirectToAction("Index", "MMCountries");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == CountryCode);
            ViewData["CountryName"] = countryName.Name;
            var mMClubsContext = _context.Province.Include(p => p.CountryCodeNavigation)
            .Where(a=>a.CountryCode == CountryCode)
            .OrderBy(a=>a.Name);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMProvinces/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // GET: MMProvinces/Create
        public IActionResult Create()
        {
            string cCode = "";
            if (Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                cCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                cCode = HttpContext.Session.GetString("CountryCode");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == cCode);
            ViewData["CountryName"] = countryName.Name;
            ViewData["CountryCode"] = countryName.CountryCode;
      //      ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode");
            return View();
        }

        // POST: MMProvinces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProvinceCode,Name,CountryCode,SalesTaxCode,SalesTax,IncludesFederalTax,FirstPostalLetter")] Province province)
        {
            string cCode = "";
            if (Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                cCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                cCode = HttpContext.Session.GetString("CountryCode");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == cCode);
            ViewData["CountryName"] = countryName.Name;


            var isDuplicate = _context.Province.Where(a => a.Name.Equals(province.Name));
            if (isDuplicate.Any())
            {
                ModelState.AddModelError("Name", "Province Name already exists");
            }
       
            if (ModelState.IsValid)
            { 
                _context.Add(province);
                await _context.SaveChangesAsync();
                TempData["message"] = "New Record added successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            return View(province);
        }

        // GET: MMProvinces/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }

            string cCode = "";
            if (Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                cCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                cCode = HttpContext.Session.GetString("CountryCode");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == cCode);
            ViewData["CountryName"] = countryName.Name;
            ViewData["CountryCode"] = countryName.CountryCode;
            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            return View(province);
        }

        // POST: MMProvinces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProvinceCode,Name,CountryCode,SalesTaxCode,SalesTax,IncludesFederalTax,FirstPostalLetter")] Province province)
        {
            if (id != province.ProvinceCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(province);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinceExists(province.ProvinceCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            string cCode = "";
            if (Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                cCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                cCode = HttpContext.Session.GetString("CountryCode");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == cCode);
            ViewData["CountryName"] = countryName.Name;
            ViewData["CountryCode"] = countryName.CountryCode;
            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            return View(province);
        }

        // GET: MMProvinces/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            string cCode = "";
            if (Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                cCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                cCode = HttpContext.Session.GetString("CountryCode");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == cCode);
            ViewData["CountryName"] = countryName.Name;
            ViewData["CountryCode"] = countryName.CountryCode;
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // POST: MMProvinces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string cCode = "";
            if (Request.Cookies["CountryCode"] != null)
            {
                //cookie check
                cCode = Request.Cookies["CountryCode"].ToString();
            }
            else if (HttpContext.Session.GetString("CountryCode") != null)
            {
                //check the session
                cCode = HttpContext.Session.GetString("CountryCode");
            }
            var countryName = _context.Country.FirstOrDefault(a => a.CountryCode == cCode);
            ViewData["CountryName"] = countryName.Name;
            ViewData["CountryCode"] = countryName.CountryCode;

            var province = await _context.Province.FindAsync(id);
            _context.Province.Remove(province);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvinceExists(string id)
        {
            return _context.Province.Any(e => e.ProvinceCode == id);
        }
    }
}
