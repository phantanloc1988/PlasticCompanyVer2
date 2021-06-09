using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Common.MyServices
{
    public interface IMyServices
    {
        void SaveFile(string folderPath, IFormFile File, string nameFile);

        void DeleteFile(string folderPath, string nameFile);
    }
}
