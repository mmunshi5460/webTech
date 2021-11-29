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
    public class MMClubStylesController : Controller
    {
        private readonly MMClubsContext _context;

        public MMClubStylesController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMClubStyles
        public async Task<IActionResult> Index()
        {
            var mMClubsContext = _context.ClubStyle.Include(c => c.Club).Include(c => c.StyleNameNavigation);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMClubStyles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubStyle = await _context.ClubStyle
                .Include(c => c.Club)
                .Include(c => c.StyleNameNavigation)
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (clubStyle == null)
            {
                return NotFound();
            }

            return View(clubStyle);
        }

        // GET: MMClubStyles/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId");
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName");
            return View();
        }

        // POST: MMClubStyles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClubId,StyleName")] ClubStyle clubStyle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubStyle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId", clubStyle.ClubId);
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName", clubStyle.StyleName);
            return View(clubStyle);
        }

        // GET: MMClubStyles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubStyle = await _context.ClubStyle.FindAsync(id);
            if (clubStyle == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId", clubStyle.ClubId);
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName", clubStyle.StyleName);
            return View(clubStyle);
        }

        // POST: MMClubStyles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClubId,StyleName")] ClubStyle clubStyle)
        {
            if (id != clubStyle.ClubId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubStyle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubStyleExists(clubStyle.ClubId))
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
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubId", clubStyle.ClubId);
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName", clubStyle.StyleName);
            return View(clubStyle);
        }

        // GET: MMClubStyles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubStyle = await _context.ClubStyle
                .Include(c => c.Club)
                .Include(c => c.StyleNameNavigation)
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (clubStyle == null)
            {
                return NotFound();
            }

            return View(clubStyle);
        }

        // POST: MMClubStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clubStyle = await _context.ClubStyle.FindAsync(id);
            _context.ClubStyle.Remove(clubStyle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubStyleExists(int id)
        {
            return _context.ClubStyle.Any(e => e.ClubId == id);
        }
    }
}
