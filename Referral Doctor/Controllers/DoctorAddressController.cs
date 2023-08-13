using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;

namespace Referral_Doctor.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DoctorAddresses == null)
            {
                return NotFound();
            }

            var doctorAddress = await _context.DoctorAddresses
                .Include(d => d.Address)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,DoctorId,AddressId,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] DoctorAddress doctorAddress)
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DoctorAddresses == null)
            {
                return NotFound();
            }

            var doctorAddress = await _context.DoctorAddresses.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,AddressId,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] DoctorAddress doctorAddress)
        {
            if (id != doctorAddress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 实现只修改Edit的内容保持其他内容不变。
                    // 在这个示例中，AsNoTracking() 方法用于从数据库中加载现有记录，但不会跟踪其更改。
                    // 然后，将现有记录的 CreatedDateTime 和 CreatedBy 的值分配给编辑后的实体，以确保在编辑操作时这些字段的值不会改变。
                    // 然后，设置 ModifiedDateTime 和 ModifiedBy 字段，将编辑后的实体保存到数据库。

                    var existingDoctorAddress = await _context.DoctorAddresses
                        .AsNoTracking() // Load the existing record without tracking changes
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingDoctorAddress == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    doctorAddress.CreatedDateTime = existingDoctorAddress.CreatedDateTime;
                    doctorAddress.CreatedBy = existingDoctorAddress.CreatedBy;

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
                    if (!DoctorAddressExists(doctorAddress.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DoctorAddresses == null)
            {
                return NotFound();
            }

            var doctorAddress = await _context.DoctorAddresses
                .Include(d => d.Address)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorAddress == null)
            {
                return NotFound();
            }

            return View(doctorAddress);
        }

        // POST: DoctorAddress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DoctorAddresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DoctorAddresses'  is null.");
            }
            var doctorAddress = await _context.DoctorAddresses.FindAsync(id);
            if (doctorAddress != null)
            {
                _context.DoctorAddresses.Remove(doctorAddress);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorAddressExists(int id)
        {
          return (_context.DoctorAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
