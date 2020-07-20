using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Models.User;

namespace Sopro.Controllers
{
    //test 
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
            if(email != null) {
            var role = IdentityProvider.getUserPriority(email);
            HttpContext.Session.SetString("email", email);
            HttpContext.Session.SetString("role", role.ToString());
            if (role != UserType.PLANER)
            {
                return RedirectToAction("Index", "Booking");
            } 
            
            return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}