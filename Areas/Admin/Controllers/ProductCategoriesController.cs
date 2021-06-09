using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlasticCompany.Areas.Admin.Services.ProductCategoriesServies;
using PlasticCompany.Areas.Admin.ViewModels.ProductCategories;
using PlasticCompany.Models;

namespace PlasticCompany.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategories _productCategories;
        public ProductCategoriesController(IProductCategories pc)
        {
            _productCategories = pc;
        }
        public IActionResult Index()
        {
            ViewBag.CategoriesList = _productCategories.GetAllProductCategories();
            return View();
        }

        //public IActionResult Create(int level)
        //{
        //    ViewBag.Level = level;
        //    ViewBag.CategoriesList = _productCategories.GetAllProductCategories();
        //    return View();
        //}

        [HttpPost]
        public IActionResult Create(ProductCategory data)
        {            
           _productCategories.CreateProductCategory(data);                             

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(ProductCategory data)
        {
            _productCategories.EditCategory(data);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _productCategories.DeleteCategory(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetCategoryById(int id)
        {
            var category = _productCategories.GetCategoryById(id);

            return Json(category);
        }

        public IActionResult GetChildrenOfCateById(int id)
        {
            var cateList = _productCategories.FindChilrenOfCategory(id);

            var listView = "/Areas/Admin/Views/Shared/ProductCategories/_Level2CateList.cshtml";

            return PartialView(listView, cateList);
        }

        public JsonResult FindChilrenOfCategory(int id)
        {
            var cateList = _productCategories.FindChilrenOfCategory(id);

            return Json(cateList);
        }
    }
}
