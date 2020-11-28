using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebView.Models;

namespace WebView.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

    }


}









//var cookie = new List<Claim>()
//            {
//             new Claim(ClaimTypes.Name, "Bob"),
//             new Claim(ClaimTypes.Email, "Bob@fmail.com"),
//             new Claim("Test.Cookie", "Admin boi")
//            };

//var licenseClaims = new List<Claim>()
//            {
//                new Claim(ClaimTypes.Email, "Bob Marlyn"),
//                new Claim("DrivingLicense", "A+")
//            };

//var cookieIdentity = new ClaimsIdentity(cookie, "Cookie Identity");
//var licenseIdentity = new ClaimsIdentity(licenseClaims, "licenseClaims Identity");

//var userPrincipal = new ClaimsPrincipal(new[] { cookieIdentity, licenseIdentity });

//HttpContext.SignInAsync(userPrincipal);

