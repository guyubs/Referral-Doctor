﻿using System;
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
    public class InsuranceCo_DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuranceCo_DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorInsurance
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InsuranceCo_Doctor.Include(d => d.Doctor).Include(d => d.InsuranceCompanies);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoctorInsurance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InsuranceCo_Doctor == null)
            {
                return NotFound();
            }

            var doctorInsurance = await _context.InsuranceCo_Doctor
                .Include(d => d.Doctor)
                .Include(d => d.InsuranceCompanies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorInsurance == null)
            {
                return NotFound();
            }

            return View(doctorInsurance);
        }

        // GET: DoctorInsurance/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId");
            ViewData["InsuranceCoId"] = new SelectList(_context.InsuranceCompanies, "InsuranceCoId", "InsuranceCoId");
            return View();
        }

        // POST: DoctorInsurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,InsuranceCoId,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] InsuranceCo_Doctor doctorInsurance)
        {
            if (ModelState.IsValid)
            {
                // 设置 CreatedDateTime 属性为当前时间
                doctorInsurance.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                doctorInsurance.CreatedBy = User.Identity.Name;

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";


                _context.Add(doctorInsurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", doctorInsurance.DoctorId);
            ViewData["InsuranceCoId"] = new SelectList(_context.InsuranceCompanies, "InsuranceCoId", "InsuranceCoId", doctorInsurance.InsuranceCoId);
            return View(doctorInsurance);
        }

        // GET: DoctorInsurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InsuranceCo_Doctor == null)
            {
                return NotFound();
            }

            var doctorInsurance = await _context.InsuranceCo_Doctor.FindAsync(id);
            if (doctorInsurance == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", doctorInsurance.DoctorId);
            ViewData["InsuranceCoId"] = new SelectList(_context.InsuranceCompanies, "InsuranceCoId", "InsuranceCoId", doctorInsurance.InsuranceCoId);
            return View(doctorInsurance);
        }

        // POST: DoctorInsurance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,InsuranceCoId,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] InsuranceCo_Doctor doctorInsurance)
        {
            if (id != doctorInsurance.Id)
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

                    var existingDoctorInsurance = await _context.InsuranceCo_Doctor
                        .AsNoTracking() // Load the existing record without tracking changes
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingDoctorInsurance == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    doctorInsurance.CreatedDateTime = existingDoctorInsurance.CreatedDateTime;
                    doctorInsurance.CreatedBy = existingDoctorInsurance.CreatedBy;

                    // Set ModifiedDateTime and ModifiedBy properties
                    doctorInsurance.ModifiedDateTime = DateTime.Now;
                    doctorInsurance.ModifiedBy = User.Identity.Name;

                    _context.Update(doctorInsurance);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorInsuranceExists(doctorInsurance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", doctorInsurance.DoctorId);
            ViewData["InsuranceCoId"] = new SelectList(_context.InsuranceCompanies, "InsuranceCoId", "InsuranceCoId", doctorInsurance.InsuranceCoId);
            return View(doctorInsurance);
        }

        // GET: DoctorInsurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InsuranceCo_Doctor == null)
            {
                return NotFound();
            }

            var doctorInsurance = await _context.InsuranceCo_Doctor
                .Include(d => d.Doctor)
                .Include(d => d.InsuranceCompanies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctorInsurance == null)
            {
                return NotFound();
            }

            return View(doctorInsurance);
        }

        // POST: DoctorInsurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InsuranceCo_Doctor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DoctorInsurances'  is null.");
            }
            var doctorInsurance = await _context.InsuranceCo_Doctor.FindAsync(id);
            if (doctorInsurance != null)
            {
                _context.InsuranceCo_Doctor.Remove(doctorInsurance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorInsuranceExists(int id)
        {
          return (_context.InsuranceCo_Doctor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
