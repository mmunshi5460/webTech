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
    public class MMClubsController : Controller
    {
        private readonly MMClubsContext _context;

        public MMClubsController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMClubs
        public async Task<IActionResult> Index()
        {
            var mMClubsContext = _context.Club.Include(c => c.ClubNavigation);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMClubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .Include(c => c.ClubNavigation)
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: MMClubs/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId");
            return View();
        }

        // POST: MMClubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubId")] Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId", club.ClubId);
            return View(club);
        }

        // GET: MMClubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId", club.ClubId);
            return View(club);
        }

        // POST: MMClubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClubId")] Club club)
        {
            if (id != club.ClubId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubId))
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
            ViewData["ClubId"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId", club.ClubId);
            return View(club);
        }

        // GET: MMClubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .Include(c => c.ClubNavigation)
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: MMClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _context.Club.FindAsync(id);
            _context.Club.Remove(club);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
            return _context.Club.Any(e => e.ClubId == id);
        }
    }
}
