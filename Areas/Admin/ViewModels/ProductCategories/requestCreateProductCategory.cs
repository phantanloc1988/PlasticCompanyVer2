using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlasticCompany.Areas.Admin.ViewModels.ProductCategories
{
    public class requestCreateProductCategory
    {
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng điền tên danh mục")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
