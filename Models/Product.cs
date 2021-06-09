using System;
using System.Collections.Generic;

namespace PlasticCompany.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public bool? Status { get; set; }
        public string Description { get; set; }
        public string MainImage { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Image6 { get; set; }
        public string Image7 { get; set; }
        public string Image8 { get; set; }
        public string Image9 { get; set; }
    }
}
