using Microsoft.AspNetCore.Http;
using PlasticCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Areas.Admin.Services.BannerServices
{
    public interface IBanner
    {
        Task<string> CreateBanner(int index, IFormFile file);

        List<Image> GetAll();

        Task<string> DeleteBanner(int index);
    }
}
