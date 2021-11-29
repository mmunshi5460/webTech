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
    public class MMArtistStylesController : Controller
    {
        private readonly MMClubsContext _context;

        public MMArtistStylesController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMArtistStyles
        public async Task<IActionResult> Index()
        {
            var mMClubsContext = _context.ArtistStyle.Include(a => a.Artist).Include(a => a.StyleNameNavigation);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMArtistStyles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistStyle = await _context.ArtistStyle
                .Include(a => a.Artist)
                .Include(a => a.StyleNameNavigation)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artistStyle == null)
            {
                return NotFound();
            }

            return View(artistStyle);
        }

        // GET: MMArtistStyles/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName");
            return View();
        }

        // POST: MMArtistStyles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,StyleName")] ArtistStyle artistStyle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistStyle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", artistStyle.ArtistId);
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName", artistStyle.StyleName);
            return View(artistStyle);
        }

        // GET: MMArtistStyles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistStyle = await _context.ArtistStyle.FindAsync(id);
            if (artistStyle == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", artistStyle.ArtistId);
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName", artistStyle.StyleName);
            return View(artistStyle);
        }

        // POST: MMArtistStyles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,StyleName")] ArtistStyle artistStyle)
        {
            if (id != artistStyle.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistStyle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistStyleExists(artistStyle.ArtistId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", artistStyle.ArtistId);
            ViewData["StyleName"] = new SelectList(_context.Style, "StyleName", "StyleName", artistStyle.StyleName);
            return View(artistStyle);
        }

        // GET: MMArtistStyles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistStyle = await _context.ArtistStyle
                .Include(a => a.Artist)
                .Include(a => a.StyleNameNavigation)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artistStyle == null)
            {
                return NotFound();
            }

            return View(artistStyle);
        }

        // POST: MMArtistStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artistStyle = await _context.ArtistStyle.FindAsync(id);
            _context.ArtistStyle.Remove(artistStyle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistStyleExists(int id)
        {
            return _context.ArtistStyle.Any(e => e.ArtistId == id);
        }
    }
}
