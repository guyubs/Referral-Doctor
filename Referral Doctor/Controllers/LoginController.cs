using Referral_Doctor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Referral_Doctor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Referral_Doctor.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 如果用户已登录，直接跳转到 Panel 页面
            if (User.Identity.IsAuthenticated)
            {
                return View("/Views/Login/Panel/Index.cshtml");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> VerifyLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null)
                {
                    // 生成ClaimsIdentity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("UserId", user.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // 使用SignInAsync登录用户
                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Panel");

                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码错误");
                    return View();
                }
            }

            // 登录失败
            return View("Index", model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            // 清除Cookie
            Response.Cookies.Delete("UserCookie");

            TempData["Message"] = "You are now logged out";

            return RedirectToAction("Index", "Home");
        }
    }
}
