using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlasticCompany.Areas.Admin.Services.InformationServices;
using PlasticCompany.Models;

namespace PlasticCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InformationController : Controller
    {
        private readonly IInformation _informationServices;
        public InformationController(IInformation info)
        {
            _informationServices = info;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> About(string data)
        {
            if (Request.Form.Count > 0)
            {
                var result = await _informationServices.CreateAbout(Request.Form["content"], (List<IFormFile>)Request.Form.Files);

                if (result == "Ok")
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Content = _informationServices.GetContact();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Contact(string content)
        {
            var result = await _informationServices.CreateContact(content);

            if (result == "Ok")
            {
                return Json("Ok");
            }
            return Json("Fail");
        }
    }
}
