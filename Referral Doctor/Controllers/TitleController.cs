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
    public class TitleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TitleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Title
        public async Task<IActionResult> Index()
        {
              return _context.Titles != null ? 
                          View(await _context.Titles.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Titles'  is null.");
        }

        // GET: Title/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .FirstOrDefaultAsync(m => m.TitleId == id);
            if (title == null)
            {
                return NotFound();
            }

            return View(title);
        }

        // GET: Title/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Title/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TitleId,TitleName,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Title title)
        {
            if (ModelState.IsValid)
            {
                // 删除输入字符串末尾的空格
                if (!string.IsNullOrWhiteSpace(title.TitleName)) // TitleName 为前端的用户输入
                {
                    title.TitleName = title.TitleName.TrimEnd();
                }

                // 设置 CreatedDateTime 属性为当前时间
                title.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                title.CreatedBy = HttpContext.Request.Cookies["Username"];

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";


                _context.Add(title);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(title);
        }

        // GET: Title/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }
            return View(title);
        }

        // POST: Title/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TitleId,TitleName,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Title title)
        {
            if (id != title.TitleId)
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

                    var existingTitle = await _context.Titles
                .AsNoTracking() // Load the existing record without tracking changes
                .FirstOrDefaultAsync(m => m.TitleId == id);

                    if (existingTitle == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    title.CreatedDateTime = existingTitle.CreatedDateTime;
                    title.CreatedBy = existingTitle.CreatedBy;

                    // Set ModifiedDateTime and ModifiedBy properties
                    title.ModifiedDateTime = DateTime.Now;
                    title.ModifiedBy = HttpContext.Request.Cookies["Username"];

                    _context.Update(title);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitleExists(title.TitleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(title);
        }

        // GET: Title/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Titles == null)
            {
                return NotFound();
            }

            var title = await _context.Titles
                .FirstOrDefaultAsync(m => m.TitleId == id);
            if (title == null)
            {
                return NotFound();
            }

            return View(title);
        }

        // POST: Title/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Titles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Titles'  is null.");
            }
            var title = await _context.Titles.FindAsync(id);
            if (title != null)
            {
                _context.Titles.Remove(title);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TitleExists(int id)
        {
          return (_context.Titles?.Any(e => e.TitleId == id)).GetValueOrDefault();
        }
    }
}
