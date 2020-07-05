using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestProjekt.Controllers
{
    public class ConnectorTypeEvaluationViewModelController : Controller
    {
        public IActionResult Evaluation()
        {
            return View();
        }
    }
}