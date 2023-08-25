using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;

namespace Referral_Doctor.Controllers
{
    [Authorize]  // 必须登录认证后次控制器才能生效
    public class NewDoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewDoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NewDoctor
        public IActionResult NewDoctor()
        {
            // 下拉菜单
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "SpecialtyId", "SpecialtyName");
            ViewData["TitleId"] = new SelectList(_context.Titles, "TitleId", "TitleName");
            ViewData["InsuranceCoId"] = new SelectList(_context.InsuranceCompanies, "InsuranceCoId", "InsuranceCoName");


            // 为了一个View同时修改到两个模型的问题，需要新增 NewDoctorViewModel
            var viewModel = new NewDoctorViewModel();
            viewModel.Doctor = new Doctor();
            viewModel.Address = new Address();
            viewModel.InsuranceCompanies = new InsuranceCompanies();

            

            return View(viewModel);
        }

        // POST: NewDoctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(NewDoctorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // 设置创建时间
                viewModel.Doctor.CreatedDateTime = DateTime.Now;
                viewModel.Doctor.CreatedBy = User.Identity.Name;
                viewModel.Address.CreatedDateTime = DateTime.Now;
                viewModel.Address.CreatedBy = User.Identity.Name;

                // 保存到数据库
                _context.Doctors.Add(viewModel.Doctor);
                await _context.SaveChangesAsync();

                _context.Addresses.Add(viewModel.Address);
                await _context.SaveChangesAsync();


                var doctorId = viewModel.Doctor.DoctorId; // 取得新生成的Id
                var addressId = viewModel.Address.AddressId;
                var insuranceCoId = viewModel.InsuranceCompanies.InsuranceCoId;

                // 创建关联记录
                var doctorAddress = new DoctorAddress
                {
                    DoctorId = doctorId,
                    AddressId = addressId
                };

                // 保存到数据库
                _context.DoctorAddresses.Add(doctorAddress);
                await _context.SaveChangesAsync();



                var insuranceCo_Doctor = new InsuranceCo_Doctor
                {
                    DoctorId = doctorId,
                    InsuranceCoId = insuranceCoId
                };

                _context.InsuranceCo_Doctor.Add(insuranceCo_Doctor);
                await _context.SaveChangesAsync();



                // 设置 页面提示信息
                TempData["success"] = "Created successfully!";

                return RedirectToAction("NewDoctor");
            }

            return View(viewModel);
        }
    }
}
