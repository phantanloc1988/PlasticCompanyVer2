using Microsoft.AspNetCore.Http;
using PlasticCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Areas.Admin.Services.ProductsServices
{
    public interface IProduct
    {
        Task<string> CreateProduct(Product data, List<IFormFile> files);

        Task<string> EditProduct(Product data, List<IFormFile> files);

        Product GetProductById(int id);

        List<Product> GetAll();
    }
}
