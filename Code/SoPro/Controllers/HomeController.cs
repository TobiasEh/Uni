using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Models.User;

namespace Sopro.Controllers
{
    /// <summary>
    /// Kontroller Klasse für das Anzeigen der Seiten ohne Funktionen.
    /// Sie regelt den Login.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Gibt die Index Ansicht zurück.
        /// </summary>
        /// <returns> Gibt die Index Ansicht zurück.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gibt die Help Ansicht zurück.
        /// </summary>
        /// <returns> Gibt die Help Ansicht zurück.</returns>
        public IActionResult Help()
        {
            return View();
        }

        /// <summary>
        /// Gibt die Imprint Ansicht zurück.
        /// </summary>
        /// <returns> Gibt die Imptint Ansicht zurück.</returns>
        public IActionResult Imprint()
        {
            return View();
        }  
        
        /// <summary>
        /// Regelt den Login.
        /// Leitet ADMINs auf die Adminansicht weiter und normale Benutzer auf die BUchungsübersichtsseite.
        /// </summary>
        /// <param name="email">E-Mail des Benutzters</param>
        /// <returns> Weiterleiten an die Jeweils zuständige Methode.</returns>
        public IActionResult Login(string email)
        {
            // Sollte eine E-Mail übergeben worden sein, wird die Rolle des Users ermittelt.
            if(email != null) {
                var role = IdentityProvider.getUserPriority(email);
                // Setzen der SessionVariable für email und role..
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("role", role.ToString());
                // Weiterleitung n die Richtige Methode.
                if (role != UserType.PLANER)
                {
                    return RedirectToAction("Index", "Booking");
                } 
            
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}