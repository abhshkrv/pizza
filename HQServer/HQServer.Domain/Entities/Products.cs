﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class Products
    {
        public string productName { get; set; }
        public int barcode { get; set; }
        public int categoryID { get; set; }
        public int manufacturerID { get; set; }
        public float costPrice { get; set; }
        public float maxPrice { get; set; }
        public int currentStock { get; set; }
        public int minimumStock { get; set; }
        public int bundleUnit { get; set; }
        public float discountPercentage { get; set; }

    }
}
