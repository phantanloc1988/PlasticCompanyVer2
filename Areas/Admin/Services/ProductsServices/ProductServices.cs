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
                    ProductCategoryId = data.ProductCategoryId,
                    Status = data.Status,
                };

                //Saave Image to DB
                var mainImageFile = files.Where(x => x.Name == "MainImage").FirstOrDefault();
                var desImageFiles = files.Where(x => x.Name != "MainImage").ToList();
                var imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", "ProducImages");


                if (mainImageFile != null)
                {
                    var mainImageName = $"{Guid.NewGuid()}-{mainImageFile.FileName}";

                    product.MainImage = mainImageName;
                    _myServices.SaveFile(imagePath, mainImageFile, mainImageName);
                }

                //Set nameFile desImage +  Add DB
                List<string> nameToSaveRootList = new List<string>(); 
                for (int i = 0; i < 9; i++)
                {                   
                    if (i < desImageFiles.Count())
                    {                       
                        nameToSaveRootList.Add(desImageFiles[i].FileName);
                    }
                    else
                    {
                        nameToSaveRootList.Add(string.Empty);
                    }
                }              

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

                //Save desImage to Root
                if (desImageFiles != null)
                {
                    for (int i = 0; i < desImageFiles.Count(); i++)
                    {
                        _myServices.SaveFile(imagePath, desImageFiles[i], desImageFiles[i].FileName);
                    }
                };

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

        public async Task<string> EditProduct(Product data, List<IFormFile> files)
        {
            using var transaction = _plasticCompanyContext.Database.BeginTransaction();

            try
            {
                var currentProduct = _plasticCompanyContext.Product.FirstOrDefault(x => x.ProductId == data.ProductId);

                var product = new Product()
                {
                    Name = data.Name,
                    Price = data.Price,
                    Sku = data.Sku,
                    Description = data.Description,
                    ProductCategoryId = data.ProductCategoryId,
                    Status = data.Status,
                };

                //Saave Image to DB
                var mainImageFile = files.Where(x => x.Name == "MainImage").FirstOrDefault();
                var desImageFiles = files.Where(x => x.Name != "MainImage").ToList();
                var imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", "ProducImages");


                if (mainImageFile != null)
                {
                    //delete old image file
                    var oldNameMainImage = currentProduct.MainImage;
                    _myServices.DeleteFile(imagePath, oldNameMainImage);

                    //create new image file
                    var newMainImageName = $"{Guid.NewGuid()}-{mainImageFile.FileName}.png";
                    product.MainImage = newMainImageName;
                    _myServices.SaveFile(imagePath, mainImageFile, newMainImageName);

                }

                //Set nameFile desImage +  Add file name to DB
                if (desImageFiles != null)
                { 
                    List<string> nameToSaveRootList = new List<string>();
                    for (int i = 0; i < 9; i++)
                    {
                        if (i < desImageFiles.Count())
                        {
                            nameToSaveRootList.Add(desImageFiles[i].FileName);

                            //create file to Root
                            _myServices.SaveFile(imagePath, desImageFiles[i], desImageFiles[i].FileName);                            
                        }
                        else
                        {
                            nameToSaveRootList.Add(string.Empty);
                        }
                    }

                    product.Image1 = nameToSaveRootList[0];
                    product.Image2 = nameToSaveRootList[1];
                    product.Image3 = nameToSaveRootList[2];
                    product.Image4 = nameToSaveRootList[3];
                    product.Image5 = nameToSaveRootList[4];
                    product.Image6 = nameToSaveRootList[5];
                    product.Image7 = nameToSaveRootList[6];
                    product.Image8 = nameToSaveRootList[7];
                    product.Image9 = nameToSaveRootList[8];

                }               

                await _plasticCompanyContext.AddAsync(product);
                _plasticCompanyContext.SaveChanges();

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
