using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Referral_Doctor.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Referral_Doctor.Controllers
{
    [Authorize]  // 必须登录认证后次控制器才能生效
    public class DoctorSchemaViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorSchemaViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = _context.Doctors
              .Join(_context.Specialties,
                d => d.SpecialtyId,
                s => s.SpecialtyId,
                (d, s) => new { Doctor = d, Specialty = s })

              .Join(_context.Titles,
                d => d.Doctor.TitleId,
                t => t.TitleId,
                (d, t) => new { Doctor = d.Doctor, Specialty = d.Specialty, Title = t })

              .Join(_context.InsuranceCo_Doctor,
                d => d.Doctor.DoctorId,
                icd => icd.DoctorId,
                (d, icd) => new { Doctor = d.Doctor, Specialty = d.Specialty, Title = d.Title, InsuranceCo_Doctor = icd })

              .Join(_context.DoctorAddresses,
                d => d.Doctor.DoctorId,
                a => a.DoctorId,
                (d, a) => new { Doctor = d.Doctor, Specialty = d.Specialty, Title = d.Title, InsuranceCo_Doctor = d.InsuranceCo_Doctor, Address = a.Address })

              .Select(x => new DoctorSchemaViewModel
              {
                  FirstName = x.Doctor.FirstName,
                  LastName = x.Doctor.LastName,

                  TitleName = x.Title.TitleName,
                  SpecialtyName = x.Specialty.SpecialtyName,

                  InsuranceCoName = x.InsuranceCo_Doctor.InsuranceCompanies.InsuranceCoName,
                  Street1 = x.Address.Street1,
                  Street2 = x.Address.Street2,
                  City = x.Address.City,
                  State = x.Address.State,
                  Zip = x.Address.Zip,
                  Tel = x.Address.Tel,
                  Fax = x.Address.Fax
              })
              .ToList();

            return View(model);
        }
    }
}

