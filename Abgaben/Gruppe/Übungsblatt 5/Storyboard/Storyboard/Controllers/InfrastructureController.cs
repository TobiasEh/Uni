using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Storyboard.Controllers
{
    public class InfrastructureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditZone()
        {
            return View();
        }
    }
}