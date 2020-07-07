using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sopro.Models.User;

namespace Sopro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
        public IActionResult Imprint()
        {
            return View();
        }      
        public IActionResult Login(string email)
        {
            if(IdentityProvider.getUserPriority(email) != UserType.PLANER)
            {
                return RedirectToAction("Index", "Booking");
            }
            return View();
        }
    }
}