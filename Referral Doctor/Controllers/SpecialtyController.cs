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
    public class SpecialtyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialtyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Specialty
        public async Task<IActionResult> Index()
        {
              return _context.Specialties != null ? 
                          View(await _context.Specialties.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Specialties'  is null.");
        }

        // GET: Specialty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specialties == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // GET: Specialty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialtyId,SpecialtyName,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                // 删除输入字符串末尾的空格
                if (!string.IsNullOrWhiteSpace(specialty.SpecialtyName)) // SpecialtyName 为前端的用户输入
                {
                    specialty.SpecialtyName = specialty.SpecialtyName.TrimEnd();
                }


                // 设置 CreatedDateTime 属性为当前时间
                specialty.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                specialty.CreatedBy = HttpContext.Request.Cookies["Username"];

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";


                _context.Add(specialty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }

        // GET: Specialty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialties == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Specialty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecialtyId,SpecialtyName,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Specialty specialty)
        {
            if (id != specialty.SpecialtyId)
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

                    var existingSpecialty = await _context.Specialties
                .AsNoTracking() // Load the existing record without tracking changes
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);

                    if (existingSpecialty == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    specialty.CreatedDateTime = existingSpecialty.CreatedDateTime;
                    specialty.CreatedBy = existingSpecialty.CreatedBy;

                    // Set ModifiedDateTime and ModifiedBy properties
                    specialty.ModifiedDateTime = DateTime.Now;
                    specialty.ModifiedBy = HttpContext.Request.Cookies["Username"];

                    _context.Update(specialty);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.SpecialtyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(specialty);
        }

        // GET: Specialty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specialties == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specialties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Specialties'  is null.");
            }
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty != null)
            {
                _context.Specialties.Remove(specialty);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyExists(int id)
        {
          return (_context.Specialties?.Any(e => e.SpecialtyId == id)).GetValueOrDefault();
        }
    }
}
