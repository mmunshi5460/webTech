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
    public class MMStylesController : Controller
    {
        private readonly MMClubsContext _context;

        //context is nothing but the database
        public MMStylesController(MMClubsContext context)
        {
            _context = context;
        }

        //This method will show the list of styles from the database
        // GET: MMStyles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Style.ToListAsync());
        }

        //This method will get the details of the style with particular style name that is selected and display it
        // GET: MMStyles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style
                .FirstOrDefaultAsync(m => m.StyleName == id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        //This method will display the create style page
        // GET: MMStyles/Create
        public IActionResult Create()
        {
            return View();
        }

        //This method will save the new style that is created by the user to the database
        // POST: MMStyles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StyleName,Description")] Style style)
        {
            if (ModelState.IsValid)
            {
                _context.Add(style);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(style);
        }

        //This method will get the style based on the style name and display it
        // GET: MMStyles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style.FindAsync(id);
            if (style == null)
            {
                return NotFound();
            }
            return View(style);
        }

        //This method will save the changes made to the selected style to the database
        // POST: MMStyles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StyleName,Description")] Style style)
        {
            if (id != style.StyleName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(style);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StyleExists(style.StyleName))
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
            return View(style);
        }

        //This method gets the style based on the style name selected/passed and displays it
        // GET: MMStyles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style
                .FirstOrDefaultAsync(m => m.StyleName == id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        ////This will save the action and remove the style from the database
        // POST: MMStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var style = await _context.Style.FindAsync(id);
            _context.Style.Remove(style);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //This method will check if the style with particular style id exists or not
        private bool StyleExists(string id)
        {
            return _context.Style.Any(e => e.StyleName == id);
        }
    }
}
