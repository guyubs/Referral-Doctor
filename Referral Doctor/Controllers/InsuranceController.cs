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
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuranceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Insurance
        public async Task<IActionResult> Index()
        {
              return _context.Insurances != null ? 
                          View(await _context.Insurances.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Insurances'  is null.");
        }

        // GET: Insurance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .FirstOrDefaultAsync(m => m.InsuranceId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceId,InsuranceName,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                // 设置 CreatedDateTime 属性为当前时间
                insurance.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                insurance.CreatedBy = HttpContext.Request.Cookies["Username"];

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";

                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insurance);
        }

        // GET: Insurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InsuranceId,InsuranceName,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Insurance insurance)
        {
            if (id != insurance.InsuranceId)
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

                    var existingInsurance = await _context.Insurances
                .AsNoTracking() // Load the existing record without tracking changes
                .FirstOrDefaultAsync(m => m.InsuranceId == id);

                    if (existingInsurance == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    insurance.CreatedDateTime = existingInsurance.CreatedDateTime;
                    insurance.CreatedBy = existingInsurance.CreatedBy;

                    // Set ModifiedDateTime and ModifiedBy properties
                    insurance.ModifiedDateTime = DateTime.Now;
                    insurance.ModifiedBy = HttpContext.Request.Cookies["Username"];

                    _context.Update(insurance);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.InsuranceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(insurance);
        }

        // GET: Insurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances
                .FirstOrDefaultAsync(m => m.InsuranceId == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Insurances == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Insurances'  is null.");
            }
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurances.Remove(insurance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
          return (_context.Insurances?.Any(e => e.InsuranceId == id)).GetValueOrDefault();
        }
    }
}
