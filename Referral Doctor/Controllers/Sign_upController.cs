using Microsoft.AspNetCore.Mvc;
using Referral_Doctor.Models;

namespace Referral_Doctor.Controllers
{
    public class Sign_upController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Sign_upController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if username or email already exists in database
                if (_context.Users.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName", "用户名已经存在");
                }
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "该电子邮件已经被注册");
                }
                // If no duplicates, save user to database
                if (ModelState.IsValid)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "注册成功";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(user);
        }
    }
}
