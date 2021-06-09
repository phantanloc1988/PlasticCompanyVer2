using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PlasticCompany.Common;
using PlasticCompany.Common.MyServices;
using PlasticCompany.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Areas.Admin.Services.ProductsServices
{
    public class ProductServices : IProduct
    {
        private readonly PlasticCompanyContext _plasticCompanyContext;
        private readonly IMyServices _myServices;
        private readonly IHostingEnvironment _WebHostEnvironment;
        public ProductServices(PlasticCompanyContext db, IMyServices ms, IHostingEnvironment wh)
        {
            _plasticCompanyContext = db;
            _myServices = ms;
            _WebHostEnvironment = wh;
        }
        public async Task<string> CreateProduct(Product data, List<IFormFile> files)
        {
            using var transaction = _plasticCompanyContext.Database.BeginTransaction();

            try
            {
                var product = new Product()
                {
                    Name = data.Name,
                    Price = data.Price,
                    Sku = data.Sku,
                    Description = data.Description,
                    ProductCategoryId = data.ProductCategoryId
                };

               

                //Saave Image to DB
                var mainImageFile = files.Where(x => x.Name == "MainImage").FirstOrDefault();
                var desImageFiles = files.Where(x => x.Name != "MainImage").ToList();

                //set Name save to Root , To not Duplicate

                var nameMainImageFile = Guid.NewGuid().ToString() + mainImageFile.FileName; //mainImage

                List<string> nameToSaveRootList = new List<string>(); // DesImages
                for (int i = 0; i < 9; i++)
                {                   
                    if (i < desImageFiles.Count())
                    {                       
                        nameToSaveRootList.Add(Guid.NewGuid().ToString() + desImageFiles[i].FileName);
                    }
                    else
                    {
                        nameToSaveRootList.Add(string.Empty);
                    }
                }              

                product.MainImage = nameMainImageFile;
                product.Image1 = nameToSaveRootList[0];
                product.Image2 = nameToSaveRootList[1];
                product.Image3 = nameToSaveRootList[2];
                product.Image4 = nameToSaveRootList[3];
                product.Image5 = nameToSaveRootList[4];
                product.Image6 = nameToSaveRootList[5];
                product.Image7 = nameToSaveRootList[6];
                product.Image8 = nameToSaveRootList[7];
                product.Image9 = nameToSaveRootList[8];

                await _plasticCompanyContext.AddAsync(product);
                _plasticCompanyContext.SaveChanges();

                //Save Image to Root
                var imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", "ProducImages");

                _myServices.SaveFile(imagePath, mainImageFile, nameMainImageFile);

                for (int i = 0; i < desImageFiles.Count(); i++)
                {
                    _myServices.SaveFile(imagePath, desImageFiles[i], nameToSaveRootList[i]);
                }                              

                transaction.Commit();

                return ("Ok");
            }
            catch (Exception e)
            {

                Tools.WriteLog(e.Message);
                Tools.WriteLog(e.StackTrace);
                Tools.WriteLog(e.Source);
                return ("asd");
            }
            
        }

        public List<Product> GetAll()
        {
            var list = _plasticCompanyContext.Product.ToList();
            return list;
        }

        public Product GetProductById(int id)
        {
            var product = _plasticCompanyContext.Product.FirstOrDefault(x => x.ProductId == id);
            return product;
        }
    }
}
