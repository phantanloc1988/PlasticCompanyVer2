using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Common.MyServices
{
    public class MyServices : IMyServices
    {
        public void SaveFile(string folderPath, IFormFile File, string nameFile)
        {
            var filePath = Path.Combine(folderPath, nameFile);

            if (File != null)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    File.CopyTo(fileStream);
                }
            }        
        }

        public void DeleteFile(string folderPath, string nameFile)
        {
            string[] image = Directory.GetFiles(folderPath, nameFile);

            File.Delete(image[0]);
        }
    }
}
