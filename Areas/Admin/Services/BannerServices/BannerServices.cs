using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PlasticCompany.Common;
using PlasticCompany.Common.MyServices;
using PlasticCompany.Models;

namespace PlasticCompany.Areas.Admin.Services.BannerServices
{
    public class BannerServices: IBanner
    {
        private readonly PlasticCompanyContext _context;
        private readonly IHostingEnvironment _WebHostEnvironment;
        private readonly IMyServices _myServices;
        public BannerServices(PlasticCompanyContext db, IHostingEnvironment we, IMyServices ms)
        {
            _context = db;
            _WebHostEnvironment = we;
            _myServices = ms;
        }
        public async Task<string> CreateBanner(int index, IFormFile file)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var nameBanner = $"Banner{index}";
                var currentBanner = _context.Image.FirstOrDefault(x => x.Name == nameBanner);
                var folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", "Other", "Banners");

                //delete old file & save new File
                if (currentBanner != null)
                {
                    var oldNameImage = currentBanner.Source;

                    _myServices.DeleteFile(folderPath, oldNameImage);

                }
                var newNameImage = $"{Guid.NewGuid()}{file.FileName}";

                _myServices.SaveFile(folderPath, file, newNameImage);

                //Update or Save banner to DB
                if (currentBanner != null)
                {
                    currentBanner.Source = newNameImage;
                    _context.Update(currentBanner);
                }
                else
                {
                    Image newBanner = new Image()
                    {
                        Area = MyConstants.ImageArea.MainPage.ToString(),
                        Type = MyConstants.ImageType.Banner.ToString(),
                        Location = MyConstants.ImageLocation.Top.ToString(),
                        Description = "Banner trang chủ",
                        Name = $"Banner{index}",
                        Source = newNameImage,
                        Index = index
                    };

                    await _context.AddAsync(newBanner);
                }

                await _context.SaveChangesAsync();

                transaction.Commit();
                return "Ok";
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.Message);
                Tools.WriteLog(e.StackTrace);
                Tools.WriteLog(e.Source);
                return "Fail";
            }         
        }

        public List<Image> GetAll()
        {
            var result = _context.Image.Where(x => x.Type == MyConstants.ImageType.Banner.ToString()).ToList();
            return result;
        }

        public async Task<string> DeleteBanner(int index)
        {
            try
            {
                using var transaction = _context.Database.BeginTransaction();

                var nameBanner = $"Banner{index}";
                var banner = _context.Image.FirstOrDefault(x => x.Name == nameBanner);
                
                if (banner != null)
                {
                    //delele DB
                    _context.Remove(banner);
                    await _context.SaveChangesAsync();

                    //delete file in Root
                    var nameImage = banner.Source;
                    var pathFolder = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", "Other");

                    _myServices.DeleteFile(pathFolder, nameImage);
                }

                transaction.Commit();
                return "Ok";
            }
            catch (Exception e)
            {
                Tools.WriteLog(e.Message);
                Tools.WriteLog(e.StackTrace);
                Tools.WriteLog(e.Source);
                return "Fail";
            }
           
        }
    }
}
