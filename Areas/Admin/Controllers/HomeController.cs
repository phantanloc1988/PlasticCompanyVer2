using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PlasticCompany.Areas.Admin.Controllers
{
    [Area("Admin")] 
    [Route("admin/[controller]")]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
