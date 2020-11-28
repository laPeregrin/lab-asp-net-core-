using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebView.ActionFilters;

namespace WebView.Controllers
{
    [LogActionFilter]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "handling " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}
