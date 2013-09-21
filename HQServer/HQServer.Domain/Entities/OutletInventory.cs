using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class OutletInventory
    {
        public int outletID { get; set; }
        public int barcode { get; set; }
        public float sellingPrice { get; set; }
        public int currentStock { get; set; }
        public int minimumStock { get; set; }

    }
}
