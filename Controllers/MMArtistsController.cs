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
    public class MMArtistsController : Controller
    {
        private readonly MMClubsContext _context;

        public MMArtistsController(MMClubsContext context)
        {
            _context = context;
        }
        /// <summary>
        /// display list of all the artists
        /// </summary>
        /// <returns></returns>
        // GET: MMArtists
        public async Task<IActionResult> Index()
        {

            var mMClubsContext = _context.Artist.Include(a => a.NameAddress);
            return View(await mMClubsContext.ToListAsync());
        }
        /// <summary>
        /// show details of a specific artist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMArtists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.NameAddress)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }
        /// <summary>
        /// create new artist
        /// </summary>
        /// <returns></returns>
        // GET: MMArtists/Create
        public IActionResult Create()
        {
            ViewData["NameAddressid"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId");
            return View();
        }
        /// <summary>
        /// Post new created artist and return to list of artists
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        // POST: MMArtists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,MinimumHourlyRate,NameAddressid")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NameAddressid"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId", artist.NameAddressid);
            return View(artist);
        }
        /// <summary>
        /// edit a specific artist details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMArtists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            ViewData["NameAddressid"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId", artist.NameAddressid);
            return View(artist);
        }
        /// <summary>
        /// post the edited artist's and display list of artists
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artist"></param>
        /// <returns></returns>
        // POST: MMArtists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,MinimumHourlyRate,NameAddressid")] Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
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
            ViewData["NameAddressid"] = new SelectList(_context.NameAddress, "NameAddressId", "NameAddressId", artist.NameAddressid);
            return View(artist);
        }
        /// <summary>
        /// Delete a specific artist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMArtists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.NameAddress)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }
        /// <summary>
        /// submit the delete and display list of existing artist list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: MMArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artist.FindAsync(id);
            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// check if artistId exists in the db or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.ArtistId == id);
        }
    }
}
