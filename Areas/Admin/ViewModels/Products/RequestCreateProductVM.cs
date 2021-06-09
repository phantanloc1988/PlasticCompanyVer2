using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using PlasticCompany.Models;

namespace PlasticCompany.Areas.Admin.ViewModels.Products
{
    public class RequestCreateProductVM
    {
        public Product Product { get; set; }     

        //public List<IFormFile> Images { get; set; }
    }
}
