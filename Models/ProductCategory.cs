using System;
using System.Collections.Generic;

namespace PlasticCompany.Models
{
    public partial class ProductCategory
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public bool? IsHasChildren { get; set; }
    }
}
