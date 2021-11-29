using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MMClubs.Models;

namespace MMClubs.Controllers
{
    public class MMNameAddressesController : Controller
    {
        private readonly MMClubsContext _context;

        public MMNameAddressesController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMNameAddresses
        public async Task<IActionResult> Index()
        {
            var mMClubsContext = _context.NameAddress.Include(n => n.ProvinceCodeNavigation);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMNameAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress
                .Include(n => n.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.NameAddressId == id);
            if (nameAddress == null)
            {
                return NotFound();
            }

            return View(nameAddress);
        }

        // GET: MMNameAddresses/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            //TempData["SuccessMessage"] = "New Record added successfully";
            return View();
        }

        // POST: MMNameAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameAddressId,FirstName,LastName,CompanyName,StreetAddress,City,PostalCode,ProvinceCode,Email,Phone")] NameAddress nameAddress)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nameAddress);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
            }
            TempData["SuccessMessage"] = "New Record added successfully";
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", nameAddress.ProvinceCode);
            return View(nameAddress);
        }

        // GET: MMNameAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress.FindAsync(id);
            if (nameAddress == null)
            {
                return NotFound();
            }
            //TempData["SuccessMessage"] = "New Record fetched successfully";
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", nameAddress.ProvinceCode);
            ViewData["ProvinceName"] = new SelectList(_context.Province.OrderBy(p => p.Name), "ProvinceCode", "Name");
            return View(nameAddress);
        }

        // POST: MMNameAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NameAddressId,FirstName,LastName,CompanyName,StreetAddress,City,PostalCode,ProvinceCode,Email,Phone")] NameAddress nameAddress)
        {
            try
            {
                if (id != nameAddress.NameAddressId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(nameAddress);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NameAddressExists(nameAddress.NameAddressId))
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
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
            }
            TempData["SuccessMessage"] = "New Record edited successfully";
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", nameAddress.ProvinceCode);
            return View(nameAddress);
        }

        // GET: MMNameAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress
                .Include(n => n.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.NameAddressId == id);
            if (nameAddress == null)
            {
                return NotFound();
            }
           // TempData["SuccessMessage"] = "New Record deleted successfully";
            return View(nameAddress);
        }

        // POST: MMNameAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var nameAddress = await _context.NameAddress.FindAsync(id);
                _context.NameAddress.Remove(nameAddress);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.GetBaseException().Message;
                RedirectToAction(nameof(Delete));
            }
            TempData["SuccessMessage"] = "New Record deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool NameAddressExists(int id)
        {
            return _context.NameAddress.Any(e => e.NameAddressId == id);
        }
    }
}
