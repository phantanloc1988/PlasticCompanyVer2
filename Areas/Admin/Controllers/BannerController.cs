using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PlasticCompany.Areas.Admin.Services.BannerServices;
using Newtonsoft.Json;

namespace PlasticCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly IBanner _bannerServices;
        public BannerController(IBanner bn)
        {
            _bannerServices = bn;
        }

        public IActionResult Index()
        {
            var banners = _bannerServices.GetAll();
            return View(banners);
        }

        [HttpPost]
        public async Task<JsonResult> Create()
        {
            if (Request.Form.Count > 0)
            {
                int index = JsonConvert.DeserializeObject<int>(Request.Form["id"]);
                var file = Request.Form.Files[0];

                var result = await _bannerServices.CreateBanner(index, (IFormFile)file);

                if (result == "Ok")
                {
                    return Json(new { status = result, url = Url.Action("Index", "Banner") });
                }
            }
            return Json(new { status = "Fail", url = Url.Action("Index", "Banner")});
        }

        public async Task<IActionResult> Delete(int index)
        {
            await _bannerServices.DeleteBanner(index);
            return RedirectToAction("Index");
        }
    }
}
