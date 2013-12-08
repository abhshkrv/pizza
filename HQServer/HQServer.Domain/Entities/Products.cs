using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace HQServer.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int productID { get; set; }
        public string productName { get; set; }
        public string barcode { get; set; }
        public int categoryID { get; set; }
        public int manufacturerID { get; set; }
        public decimal costPrice { get; set; }
        public decimal maxPrice { get; set; }
        public int currentStock { get; set; }
        public int minimumStock { get; set; }
        public int bundleUnit { get; set; }
        public decimal discountPercentage { get; set; }
    }
}
