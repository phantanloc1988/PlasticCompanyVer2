using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlasticCompany.Areas.Admin.Services.ProductCategoriesServies;
using PlasticCompany.Areas.Admin.Services.ProductsServices;
using PlasticCompany.Areas.Admin.ViewModels.Products;
using PlasticCompany.Common.MyServices;
using PlasticCompany.Models;
using X.PagedList;

namespace PlasticCompany.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IMyServices _myServcies;
        private readonly IProduct _productServices;
        private readonly IProductCategories _productCategoriesServices;

        public ProductsController(IMyServices ms, IProduct p, IProductCategories pc)
        {
            _myServcies = ms;
            _productServices = p;
            _productCategoriesServices = pc;
        }
        public IActionResult Index(int? page)
        {
            var productList = _productServices.GetAll();
            var pageNumber = page ?? 1; 
            var onePageOfProducts = productList.ToPagedList(pageNumber, 5);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(onePageOfProducts);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _productCategoriesServices.GetAllProductCategories();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(string data)
        {
            if (Request.Form.Count > 0)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Request.Form["Product"]);
                var files = Request.Form.Files;

                var result = await _productServices.CreateProduct(product, (List<IFormFile>)files);

                if (result == "Ok")
                {
                    return Json(new { status = result , url = Url.Action("Index","Products")});
                }
            }
            return Json(new { status = "Fail", url = Url.Action("Create", "Products") });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productServices.GetProductById(id);
            ViewBag.Categories = _productCategoriesServices.GetAllProductCategories();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string data)
        {
            if (Request.Form.Count > 0)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Request.Form["Product"]);
                var files = Request.Form.Files;

                var result = await _productServices.EditProduct(product, (List<IFormFile>)files);

                if (result == "Ok")
                {
                    return Json(new { status = result, url = Url.Action("Index", "Products") });
                }
            }
            return Json(new { status = "Fail", url = Url.Action("Create", "Products") });
        }
    }
}
