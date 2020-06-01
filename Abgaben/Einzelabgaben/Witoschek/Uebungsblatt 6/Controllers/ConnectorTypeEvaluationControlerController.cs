using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blatt03.Controllers
{
    public class ConnectorTypeEvaluationControlerController : Controller
    {
        public IActionResult Overview()
        {
            return View();
        }
    }
}