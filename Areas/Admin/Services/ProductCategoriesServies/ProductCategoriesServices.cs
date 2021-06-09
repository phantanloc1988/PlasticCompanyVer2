using Microsoft.AspNetCore.Http;
using PlasticCompany.Areas.Admin.ViewModels.ProductCategories;
using PlasticCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Areas.Admin.Services.ProductCategoriesServies
{
    public class ProductCategoriesServices : IProductCategories
    {
        private readonly PlasticCompanyContext _plasticCompanyContext;
        public ProductCategoriesServices(PlasticCompanyContext db)
        {
            _plasticCompanyContext = db;
        }
        public void CreateProductCategory(ProductCategory data)
        {
            using var transaction = _plasticCompanyContext.Database.BeginTransaction();
            try
            {
                if (data.Name != null)
                {
                    var parentId = 0;

                    if (data.ParentId != null)
                    {
                        parentId = (int)data.ParentId;
                    }

                    var newItem = new ProductCategory()
                    {
                        Name = data.Name,
                        Level = data.Level,
                        ParentId = parentId,
                        IsHasChildren = false
                    };

                    //case: level 2
                    //change isHasChildren for cate level 1
                    var cateLevel1 = _plasticCompanyContext.ProductCategory.FirstOrDefault(x => x.ProductCategoryId == data.ParentId);

                    if (data.Level == 2 && cateLevel1.IsHasChildren == false)
                    {
                        cateLevel1.IsHasChildren = true;
                        _plasticCompanyContext.ProductCategory.Update(cateLevel1);
                    }

                    _plasticCompanyContext.ProductCategory.Add(newItem);
                    _plasticCompanyContext.SaveChanges();

                    transaction.Commit();
                }
            }
            catch (Exception)
            {

                throw;
            }
                                      
        }

        public void DeleteCategory(int id)
        {
            using var transaction = _plasticCompanyContext.Database.BeginTransaction();
            try
            {
                var category = _plasticCompanyContext.ProductCategory.Find(id);
                var children = this.FindChilrenOfCategory(id);

                if (children.Count > 0)
                {
                    foreach (var item in children)
                    {
                        _plasticCompanyContext.Remove(item);
                    }
                }
                _plasticCompanyContext.Remove(category);
                _plasticCompanyContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public async void EditCategory(ProductCategory data)
        {
            if (data != null)
            {
                var category = _plasticCompanyContext.ProductCategory.Where(x => x.ProductCategoryId == data.ProductCategoryId).FirstOrDefault();

                category.Name = data.Name;
                category.Level = data.Level;
                category.ParentId = data.ParentId;

                _plasticCompanyContext.ProductCategory.Update(category);
                await _plasticCompanyContext.SaveChangesAsync();
            }
        }

        public ProductCategory GetCategoryById(int id) 
        {
            var category = _plasticCompanyContext.ProductCategory.Where(x => x.ProductCategoryId == id).FirstOrDefault(); ;

            return category;
        }

        public List<ProductCategory> FindChilrenOfCategory(int id)
        {
            var result = _plasticCompanyContext.ProductCategory.Where(x => x.ParentId == id).ToList();

            return result;
        }

        public List<ProductCategory> GetAllProductCategories()
        {
            return _plasticCompanyContext.ProductCategory.ToList();
        }        
    }
}
