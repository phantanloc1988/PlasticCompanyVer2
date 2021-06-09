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

namespace PlasticCompany.Areas.Admin.Services.InformationServices
{
    public class InformationServices : IInformation
    {
        private readonly PlasticCompanyContext _plasticCompanyContext;
        private readonly IMyServices _myServices;
        private readonly IHostingEnvironment _WebHostEnvironment;

        public InformationServices(PlasticCompanyContext db, IMyServices ms, IHostingEnvironment we)
        {
            _plasticCompanyContext = db;
            _myServices = ms;
            _WebHostEnvironment = we;
        }
        public async Task<string> CreateAbout(string data, List<IFormFile> files)
        {
            using var transaction = _plasticCompanyContext.Database.BeginTransaction();
            try
            {
                //Add info
                About newAbout = new About()
                {
                    Content = data
                };

                await _plasticCompanyContext.AddAsync(newAbout);
                await _plasticCompanyContext.SaveChangesAsync();

                //Save image files
                foreach (var item in files)
                {
                    var nameFile = $"{Guid.NewGuid()}-{item.FileName}";

                    //add DB
                    Image image = new Image()
                    {
                        Type = MyConstants.ImageType.AboutInformation.ToString(),
                        Name = nameFile
                    };

                    //add root
                    var folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, "Images", "AboutImages");

                    _myServices.SaveFile(folderPath, item, nameFile);
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

        public async Task<string> CreateContact(string content)
        {
            try
            {
                var contactObject = new Contact()
                {
                    Content = content
                };

                await _plasticCompanyContext.AddAsync(contactObject);
                await _plasticCompanyContext.SaveChangesAsync();

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

        public string GetContact()
        {
           var obj = _plasticCompanyContext.Contact.Where(x => x.ContactId == 0).FirstOrDefault();

            var result = obj == null || obj.Content == null ? "Chưa tạo nội dung" : obj.Content ;
            return result;
        }
    }
}
