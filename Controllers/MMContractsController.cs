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
    public class MMContractsController : Controller
    {
        private readonly MMClubsContext _context;

        public MMContractsController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMContracts
        public async Task<IActionResult> Index()
        {
            var mMClubsContext = _context.Contract.Include(c => c.Artist).Include(c => c.Club);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Artist)
                .Include(c => c.Club)
                .FirstOrDefaultAsync(m => m.Contract1 == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: MMContracts/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId");
            return View();
        }

        // POST: MMContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Contract1,StartDate,ArtistId,ClubId,PricePerPerformance,NumberPerformances,TotalPrice")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", contract.ArtistId);
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId", contract.ClubId);
            return View(contract);
        }

        // GET: MMContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", contract.ArtistId);
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId", contract.ClubId);
            return View(contract);
        }

        // POST: MMContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Contract1,StartDate,ArtistId,ClubId,PricePerPerformance,NumberPerformances,TotalPrice")] Contract contract)
        {
            if (id != contract.Contract1)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Contract1))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", contract.ArtistId);
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId", contract.ClubId);
            return View(contract);
        }

        // GET: MMContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Artist)
                .Include(c => c.Club)
                .FirstOrDefaultAsync(m => m.Contract1 == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: MMContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            _context.Contract.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.Contract1 == id);
        }
    }
}
