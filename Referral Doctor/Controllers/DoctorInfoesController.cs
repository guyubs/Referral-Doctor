using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;

namespace Referral_Doctor.Controllers
{
    [Authorize]
    public class DoctorInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorInfoes
        public async Task<IActionResult> Index()
        {
              return _context.DoctorInfos != null ? 
                          View(await _context.DoctorInfos.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DoctorInfos'  is null.");
        }

        // GET: DoctorInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DoctorInfos == null)
            {
                return NotFound();
            }

            var doctorInfo = await _context.DoctorInfos
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctorInfo == null)
            {
                return NotFound();
            }

            return View(doctorInfo);
        }

        // GET: DoctorInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DoctorInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,FirstName,LastName,Cell,TitleName,SpecialtyName,InsuranceCoName,Address,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] DoctorInfo doctorInfo)
        {
            if (ModelState.IsValid)
            {
                // 删除输入字符串末尾的空格
                if (!string.IsNullOrWhiteSpace(doctorInfo.FirstName))
                {
                    doctorInfo.FirstName = doctorInfo.FirstName.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(doctorInfo.LastName))
                {
                    doctorInfo.LastName = doctorInfo.LastName.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(doctorInfo.Cell))
                    doctorInfo.Cell = doctorInfo.Cell.TrimEnd();

                if (!string.IsNullOrWhiteSpace(doctorInfo.TitleName))
                {
                    doctorInfo.TitleName = doctorInfo.TitleName.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(doctorInfo.SpecialtyName))
                {
                    doctorInfo.SpecialtyName = doctorInfo.SpecialtyName.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(doctorInfo.InsuranceCoName))
                {
                    doctorInfo.InsuranceCoName = doctorInfo.InsuranceCoName.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(doctorInfo.Address))
                {
                    doctorInfo.Address = doctorInfo.Address.TrimEnd();
                }


                // 设置 CreatedDateTime 属性为当前时间
                doctorInfo.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                doctorInfo.CreatedBy = User.Identity.Name;

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";

                _context.Add(doctorInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(doctorInfo);
        }

        // GET: DoctorInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DoctorInfos == null)
            {
                return NotFound();
            }

            var doctorInfo = await _context.DoctorInfos.FindAsync(id);
            if (doctorInfo == null)
            {
                return NotFound();
            }
            return View(doctorInfo);
        }

        // POST: DoctorInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorID,FirstName,LastName,Cell,TitleName,SpecialtyName,InsuranceCoName,Address,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] DoctorInfo doctorInfo)
        {
            if (id != doctorInfo.DoctorID)
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

                    var existingdoctorInfo = await _context.DoctorInfos
                    .AsNoTracking() // Load the existing record without tracking changes
                    .FirstOrDefaultAsync(m => m.DoctorID == id);

                    if (existingdoctorInfo == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    doctorInfo.CreatedDateTime = existingdoctorInfo.CreatedDateTime;
                    doctorInfo.CreatedBy = existingdoctorInfo.CreatedBy;

                    // Set ModifiedDateTime and ModifiedBy properties
                    doctorInfo.ModifiedDateTime = DateTime.Now;
                    doctorInfo.ModifiedBy = User.Identity.Name;


                    // 删除输入字符串末尾的空格
                    if (!string.IsNullOrWhiteSpace(doctorInfo.FirstName))
                    {
                        doctorInfo.FirstName = doctorInfo.FirstName.TrimEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(doctorInfo.LastName))
                    {
                        doctorInfo.LastName = doctorInfo.LastName.TrimEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(doctorInfo.Cell))
                        doctorInfo.Cell = doctorInfo.Cell.TrimEnd();

                    if (!string.IsNullOrWhiteSpace(doctorInfo.TitleName))
                    {
                        doctorInfo.TitleName = doctorInfo.TitleName.TrimEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(doctorInfo.SpecialtyName))
                    {
                        doctorInfo.SpecialtyName = doctorInfo.SpecialtyName.TrimEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(doctorInfo.InsuranceCoName))
                    {
                        doctorInfo.InsuranceCoName = doctorInfo.InsuranceCoName.TrimEnd();
                    }

                    if (!string.IsNullOrWhiteSpace(doctorInfo.Address))
                    {
                        doctorInfo.Address = doctorInfo.Address.TrimEnd();
                    }

                   
                    // 设置 CreatedDateTime 属性为当前时间
                    doctorInfo.ModifiedDateTime = DateTime.Now;

                    // 设置 CreatedBy
                    doctorInfo.ModifiedBy = User.Identity.Name;

                    // 设置 页面提示信息
                    TempData["success"] = "Created successfully!";



                    _context.Update(doctorInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorInfoExists(doctorInfo.DoctorID))
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
            return View(doctorInfo);
        }

        // GET: DoctorInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DoctorInfos == null)
            {
                return NotFound();
            }

            var doctorInfo = await _context.DoctorInfos
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctorInfo == null)
            {
                return NotFound();
            }

            return View(doctorInfo);
        }

        // POST: DoctorInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DoctorInfos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DoctorInfos'  is null.");
            }
            var doctorInfo = await _context.DoctorInfos.FindAsync(id);
            if (doctorInfo != null)
            {
                _context.DoctorInfos.Remove(doctorInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorInfoExists(int id)
        {
          return (_context.DoctorInfos?.Any(e => e.DoctorID == id)).GetValueOrDefault();
        }
    }
}
