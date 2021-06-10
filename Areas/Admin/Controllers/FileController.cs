
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PlasticCompany.Common.MyServices;
using System.IO;

namespace PlasticCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileController : Controller
    {
        private readonly IMyServices _myServcies;
        private readonly IHostingEnvironment _webHostEnvironment;
        public FileController(IMyServices ms , IHostingEnvironment he)
        {
            _myServcies = ms;
            _webHostEnvironment = he;
        }
        [HttpPost]
        public JsonResult SaveImageFromSummernote()
        {
            if (Request.Form.Files.Count > 0)
            {
                var Images = Request.Form.Files;
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "ProductImage");

            }
            return Json("asd");
        }
    }
}
