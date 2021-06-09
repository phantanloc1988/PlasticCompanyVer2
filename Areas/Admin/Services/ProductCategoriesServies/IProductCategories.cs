using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlasticCompany.Areas.Admin.ViewModels.ProductCategories;
using PlasticCompany.Models;
namespace PlasticCompany.Areas.Admin.Services.ProductCategoriesServies
{
    public interface IProductCategories
    {
        public void CreateProductCategory(ProductCategory newProductCategory);
        public void DeleteCategory(int id);

        public List<ProductCategory> FindChilrenOfCategory(int id);

        public List<ProductCategory> GetAllProductCategories();

        public ProductCategory GetCategoryById(int id);

        void EditCategory(ProductCategory data);
    }
}
