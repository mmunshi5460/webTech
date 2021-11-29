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
    public class MMArtistInstrumentsController : Controller
    {
        private readonly MMClubsContext _context;

        public MMArtistInstrumentsController(MMClubsContext context)
        {
            _context = context;
        }

        // GET: MMArtistInstruments
        public async Task<IActionResult> Index()
        {
            var mMClubsContext = _context.ArtistInstrument.Include(a => a.Artist).Include(a => a.Instrument);
            return View(await mMClubsContext.ToListAsync());
        }

        // GET: MMArtistInstruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistInstrument = await _context.ArtistInstrument
                .Include(a => a.Artist)
                .Include(a => a.Instrument)
                .FirstOrDefaultAsync(m => m.ArtistInstrumentId == id);
            if (artistInstrument == null)
            {
                return NotFound();
            }

            return View(artistInstrument);
        }

        // GET: MMArtistInstruments/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "InstrumentId");
            return View();
        }

        // POST: MMArtistInstruments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistInstrumentId,ArtistId,InstrumentId")] ArtistInstrument artistInstrument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistInstrument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", artistInstrument.ArtistId);
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "InstrumentId", artistInstrument.InstrumentId);
            return View(artistInstrument);
        }

        // GET: MMArtistInstruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistInstrument = await _context.ArtistInstrument.FindAsync(id);
            if (artistInstrument == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", artistInstrument.ArtistId);
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "InstrumentId", artistInstrument.InstrumentId);
            return View(artistInstrument);
        }

        // POST: MMArtistInstruments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistInstrumentId,ArtistId,InstrumentId")] ArtistInstrument artistInstrument)
        {
            if (id != artistInstrument.ArtistInstrumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistInstrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistInstrumentExists(artistInstrument.ArtistInstrumentId))
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
            ViewData["ArtistId"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", artistInstrument.ArtistId);
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "InstrumentId", artistInstrument.InstrumentId);
            return View(artistInstrument);
        }

        // GET: MMArtistInstruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistInstrument = await _context.ArtistInstrument
                .Include(a => a.Artist)
                .Include(a => a.Instrument)
                .FirstOrDefaultAsync(m => m.ArtistInstrumentId == id);
            if (artistInstrument == null)
            {
                return NotFound();
            }

            return View(artistInstrument);
        }

        // POST: MMArtistInstruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artistInstrument = await _context.ArtistInstrument.FindAsync(id);
            _context.ArtistInstrument.Remove(artistInstrument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistInstrumentExists(int id)
        {
            return _context.ArtistInstrument.Any(e => e.ArtistInstrumentId == id);
        }
    }
}
