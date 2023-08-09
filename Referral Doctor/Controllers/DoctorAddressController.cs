using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;
using ReferralDoctor.Models;

namespace Referral_Doctor.Controllers
{
    public class DoctorAddressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorAddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorAddress
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DoctorAddresses.Include(d => d.Address).Include(d => d.Doctor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoctorAddress/Details/5
        public async Task<IActionResult> Details(int? doctorId, int? addressId)
        {
            if (doctorId == null || addressId == null || _context.DoctorAddresses == null)
            {
                return NotFound();
            }

            var doctorAddress = await _context.DoctorAddresses
                .Include(d => d.Address)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.DoctorId == doctorId && m.AddressId == addressId);

            if (doctorAddress == null)
            {
                return NotFound();
            }

            return View(doctorAddress);
        }

        // GET: DoctorAddress/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId");
            return View();
        }

        // POST: DoctorAddress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,AddressId,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] DoctorAddress doctorAddress)
        {
            if (ModelState.IsValid)
            {
                // 设置 CreatedDateTime 属性为当前时间
                doctorAddress.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                doctorAddress.CreatedBy = HttpContext.Request.Cookies["Username"];

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";


                _context.Add(doctorAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", doctorAddress.AddressId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", doctorAddress.DoctorId);
            return View(doctorAddress);
        }

        // GET: DoctorAddress/Edit/5
        public async Task<IActionResult> Edit(int? doctorId, int? addressId)
        {
            if (doctorId == null || addressId == null || _context.DoctorAddresses == null)
            {
                return NotFound();
            }

            var doctorAddress = await _context.DoctorAddresses.FindAsync(doctorId, addressId);
            if (doctorAddress == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", doctorAddress.AddressId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", doctorAddress.DoctorId);
            return View(doctorAddress);
        }

        // POST: DoctorAddress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int doctorId, int addressId, [Bind("DoctorId,AddressId,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] DoctorAddress doctorAddress)
        {
            if (doctorId != doctorAddress.DoctorId || addressId != doctorAddress.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 我们首先加载现有记录，然后将 CreatedBy 和 CreatedDateTime 的值从现有记录复制到编辑后的实体中，
                    // 以确保它们的值不会因为编辑操作而改变。然后我们继续执行更新操作。
                    // Load the existing record without tracking changes
                    var existingDoctorAddress = await _context.DoctorAddresses
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.DoctorId == doctorId && m.AddressId == addressId);

                    if (existingDoctorAddress == null)
                    {
                        return NotFound();
                    }

                    // Preserve the values of CreatedBy and CreatedDateTime
                    doctorAddress.CreatedBy = existingDoctorAddress.CreatedBy;
                    doctorAddress.CreatedDateTime = existingDoctorAddress.CreatedDateTime;

                    // Set ModifiedDateTime and ModifiedBy properties
                    doctorAddress.ModifiedDateTime = DateTime.Now;
                    doctorAddress.ModifiedBy = HttpContext.Request.Cookies["Username"];

                    _context.Update(doctorAddress);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorAddressExists(doctorId, addressId))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", doctorAddress.AddressId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", doctorAddress.DoctorId);
            return View(doctorAddress);
        }

        // GET: DoctorAddress/Delete/5
        public async Task<IActionResult> Delete(int? doctorId, int? addressId)
        {
            if (doctorId == null || addressId == null || _context.DoctorAddresses == null)
            {
                return NotFound();
            }

            var doctorAddress = await _context.DoctorAddresses
                .Include(d => d.Address)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.DoctorId == doctorId && m.AddressId == addressId);
            if (doctorAddress == null)
            {
                return NotFound();
            }

            return View(doctorAddress);
        }

        // POST: DoctorAddress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int doctorId, int addressId)
        {
            if (_context.DoctorAddresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DoctorAddresses' is null.");
            }
            var doctorAddress = await _context.DoctorAddresses.FindAsync(doctorId, addressId);
            if (doctorAddress != null)
            {
                _context.DoctorAddresses.Remove(doctorAddress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorAddressExists(int doctorId, int addressId)
        {
            return (_context.DoctorAddresses?.Any(e => e.DoctorId == doctorId && e.AddressId == addressId)).GetValueOrDefault();
        }
    }
}
