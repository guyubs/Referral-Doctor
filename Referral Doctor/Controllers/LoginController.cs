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
                    /////// 生成ClaimsIdentity ////////////
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("UserId", user.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    
                    // 使用SignInAsync登录用户
                    await HttpContext.SignInAsync(principal);
                    ////////////////////////////////////////////

                    //////////// 添加登录历史记录 //////////
                    var loginHistory = new LoginHistory
                    {
                        Timestamp = DateTime.Now,
                        Ip_addr = GetIpAddress(), // 该自定义方法在代码最后。
                        Username = user.UserName
                    };

                    _context.LoginHistory.Add(loginHistory);
                    await _context.SaveChangesAsync();
                    //////////////////////////////////////

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

        // 获得用户IP
        public string GetIpAddress()
        {
            // 获取客户端的 IP 地址
            var ipAddress = HttpContext.Connection.RemoteIpAddress;

            // 获取 IPv4 或 IPv6 地址的字符串表示
            var ipString = ipAddress.MapToIPv4().ToString(); // 如果您确定客户端的 IP 是 IPv4
                                                             // var ipString = ipAddress.ToString(); // 如果客户端的 IP 可能是 IPv4 或 IPv6

            return ipString;
        }
    }
}
