using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;

namespace Referral_Doctor.Controllers
{
    public class LoginHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoginHistory
        public async Task<IActionResult> Index()
        {
              return _context.LoginHistory != null ? 
                          View(await _context.LoginHistory.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.LoginHistory'  is null.");
        }

        // GET: LoginHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LoginHistory == null)
            {
                return NotFound();
            }

            var loginHistory = await _context.LoginHistory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loginHistory == null)
            {
                return NotFound();
            }

            return View(loginHistory);
        }

        // GET: LoginHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoginHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Timestamp,Ip_addr,Username")] LoginHistory loginHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loginHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loginHistory);
        }

        // GET: LoginHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoginHistory == null)
            {
                return NotFound();
            }

            var loginHistory = await _context.LoginHistory.FindAsync(id);
            if (loginHistory == null)
            {
                return NotFound();
            }
            return View(loginHistory);
        }

        // POST: LoginHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Timestamp,Ip_addr,Username")] LoginHistory loginHistory)
        {
            if (id != loginHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loginHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginHistoryExists(loginHistory.Id))
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
            return View(loginHistory);
        }

        // GET: LoginHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoginHistory == null)
            {
                return NotFound();
            }

            var loginHistory = await _context.LoginHistory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loginHistory == null)
            {
                return NotFound();
            }

            return View(loginHistory);
        }

        // POST: LoginHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoginHistory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LoginHistory'  is null.");
            }
            var loginHistory = await _context.LoginHistory.FindAsync(id);
            if (loginHistory != null)
            {
                _context.LoginHistory.Remove(loginHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginHistoryExists(int id)
        {
          return (_context.LoginHistory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
