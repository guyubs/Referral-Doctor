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
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
              return _context.Addresses != null ? 
                          View(await _context.Addresses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Addresses'  is null.");
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,Street1,Street2,City,State,Zip,Tel,Fax,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Address address)
        {
            if (ModelState.IsValid)
            {
                // 删除输入字符串末尾的空格
                if (!string.IsNullOrWhiteSpace(address.Street1)) // TitleName 为前端的用户输入
                {
                    address.Street1 = address.Street1.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(address.Street2)) // TitleName 为前端的用户输入
                {
                    address.Street2 = address.Street2.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(address.City)) // TitleName 为前端的用户输入
                {
                    address.City = address.City.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(address.State)) // TitleName 为前端的用户输入
                {
                    address.State = address.State.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(address.Zip)) // TitleName 为前端的用户输入
                {
                    address.Zip = address.Zip.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(address.Tel)) // TitleName 为前端的用户输入
                {
                    address.Tel = address.Tel.TrimEnd();
                }

                if (!string.IsNullOrWhiteSpace(address.Fax)) // TitleName 为前端的用户输入
                {
                    address.Fax = address.Fax.TrimEnd();
                }


                // 设置 CreatedDateTime 属性为当前时间
                address.CreatedDateTime = DateTime.Now;

                // 设置 CreatedBy
                address.CreatedBy = HttpContext.Request.Cookies["Username"];

                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";


                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressId,Street1,Street2,City,State,Zip,Tel,Fax,Note,Deleted,CreatedBy,ModifiedBy,CreatedDateTime,ModifiedDateTime")] Address address)
        {
            if (id != address.AddressId)
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

                    var existingAddress = await _context.Addresses
                .AsNoTracking() // Load the existing record without tracking changes
                .FirstOrDefaultAsync(m => m.AddressId == id);

                    if (existingAddress == null)
                    {
                        return NotFound();
                    }

                    // Assign the values of CreatedDateTime and CreatedBy from the existing record
                    address.CreatedDateTime = existingAddress.CreatedDateTime;
                    address.CreatedBy = existingAddress.CreatedBy;

                    // Set ModifiedDateTime and ModifiedBy properties
                    address.ModifiedDateTime = DateTime.Now;
                    address.ModifiedBy = HttpContext.Request.Cookies["Username"];

                    _context.Update(address);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(address);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Addresses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Addresses'  is null.");
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
          return (_context.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
