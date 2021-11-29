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
    public class MMGroupMembersController : Controller
    {
        private readonly MMClubsContext _context;

        public MMGroupMembersController(MMClubsContext context)
        {
            _context = context;
        }
        /// <summary>
        /// check for id in session/cookie or query, store it if it is missing
        /// differentiate between members of group/individual and display corresponding page
        /// </summary>
        /// <param name="ArtistId"></param>
        /// <returns></returns>
        // GET: MMGroupMembers
        public async Task<IActionResult> Index(string ArtistId)
        {
            if (!string.IsNullOrEmpty(ArtistId))
            {
                //store in cookie or session
                Response.Cookies.Append("", ArtistId);
                HttpContext.Session.SetString("ArtistId", ArtistId);

            }
            else if (Request.Query["ArtistId"].Any())
            {
                ArtistId = Request.Query["ArtistId"].ToString();
                //store in cookie or session
                Response.Cookies.Append("", ArtistId);
                HttpContext.Session.SetString("ArtistId", ArtistId);
            }
            else if (Request.Cookies["ArtistId"] != null)
            {
                //cookie check
                ArtistId = Request.Cookies["ArtistId"].ToString();
            }
            else if (HttpContext.Session.GetString("ArtistId") != null)
            {
                //check the session
                ArtistId = HttpContext.Session.GetString("ArtistId");
            }
            else
            {
                //Redirecting back to Artists with the message
                TempData["message"] = "Please select an Artist";
                return RedirectToAction("Index", "MMArtistsController");
            }

            var groupArtist =await _context.GroupMember.Where(m => m.ArtistIdGroup.Equals(Convert.ToInt32(ArtistId))).ToListAsync();
            var memberArtist =await _context.GroupMember.Where(m => m.ArtistIdMember.Equals(Convert.ToInt32(ArtistId))).ToListAsync();
            var memberName = _context.NameAddress.Find(Convert.ToInt32(ArtistId));
            if (groupArtist.Count > 0)
            {
                var _groupArtist =await _context.GroupMember.Where(m => m.ArtistIdGroup.Equals(Convert.ToInt32(ArtistId)))
                    .Include(m => m.ArtistIdGroupNavigation)
                    .Include(m => m.ArtistIdGroupNavigation.NameAddress)
                    .OrderBy(m => m.DateLeft)
                    .ThenBy(m => m.DateJoined)
                    .ToListAsync();

                ViewBag.Header = $"Members of '{memberName.FullName}'" ;
                TempData["artistId"] = ArtistId;
                return View(_groupArtist);
            }else if(memberArtist.Count > 0)
            {
                var _memberArtist =await _context.GroupMember.Where(m => m.ArtistIdMember.Equals(Convert.ToInt32(ArtistId)))
                    .Include(m => m.ArtistIdMemberNavigation)
                    .Include(m => m.ArtistIdGroupNavigation.NameAddress)
                    .OrderBy(m => m.DateLeft)
                    .ThenBy(m => m.DateJoined)
                    .ToListAsync();
                
                TempData["Message"] = "Artist is an individual, not a group, so here’s their historic group memberships";
                ViewBag.Header = $"Groups with '{memberName.FullName}'";
                return View("MMGroupsForArtist", _memberArtist);
            }else //redirecting the user to the GroupMemberController’s Create action
            {
                ViewData["Message"] = "The artist is neither a group nor a group member, but they can become a group";
                return View("Create");
            }

        }
        /// <summary>
        /// show details of particular group and member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMGroupMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
            if (groupMember == null)
            {
                return NotFound();
            }
            var memberName = _context.NameAddress.Find(Convert.ToInt32(id));
            ViewBag.Header = $"Details of '{memberName.FullName}'";
            return View(groupMember);
        }
        /// <summary>
        /// create new member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMGroupMembers/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            var memberName = _context.NameAddress.Find(Convert.ToInt32(id));
            ViewBag.Header = $"Add member to the '{memberName.FullName}'";
            return View();
        }
        /// <summary>
        /// post the new member information and show the index including new member
        /// </summary>
        /// <param name="groupMember"></param>
        /// <returns></returns>
        // POST: MMGroupMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistIdGroup,ArtistIdMember,DateJoined,DateLeft")] GroupMember groupMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdMember);
           // var memberName = _context.NameAddress.Find(Convert.ToInt32(id));
            //ViewData["Name"] = $"Add member to the '{memberName.FullName}'";
            return View(groupMember);
        }
        /// <summary>
        /// display edit page for the particular member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMGroupMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
           // var groupMember = await _context.GroupMember.FindAsync(id);
            if (groupMember == null)
            {
                return NotFound();
            }
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdMember);
            var memberName = _context.NameAddress.Find(Convert.ToInt32(id));
            ViewBag.Header = $"Edit member of the '{memberName.FullName}'";
            return View(groupMember);
        }
        /// <summary>
        /// post the edited information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupMember"></param>
        /// <returns></returns>
        // POST: MMGroupMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistIdGroup,ArtistIdMember,DateJoined,DateLeft")] GroupMember groupMember)
        {
            if (id != groupMember.ArtistIdGroup)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupMemberExists(groupMember.ArtistIdGroup))
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
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            ViewData["ArtistIdMember"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdMember);
            
            return View(groupMember);
        }
        /// <summary>
        /// go to delete page for the particular member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: MMGroupMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
            if (groupMember == null)
            {
                return NotFound();
            }
            var memberName = _context.NameAddress.Find(Convert.ToInt32(id));
            ViewBag.Header = $"Delete member of the '{memberName.FullName}'?";
            return View(groupMember);
        }
        /// <summary>
        /// delete member of the group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: MMGroupMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupMember = await _context.GroupMember.FindAsync(id);
            _context.GroupMember.Remove(groupMember);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool GroupMemberExists(int id)
        {
            return _context.GroupMember.Any(e => e.ArtistIdGroup == id);
        }
    }
}
