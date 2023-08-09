using Microsoft.AspNetCore.Mvc;
using Referral_Doctor.Models;
using System.Diagnostics;

namespace Referral_Doctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // 点击Home页Sign_up 显示/Views/Home/Sign_up/Index.cshtml
        //public IActionResult Sign_up()
        //{
        // return View("/Views/Home/Sign_up/Index.cshtml");
        //}



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}