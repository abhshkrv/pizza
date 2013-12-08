using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class OutletInventory
    {
        [Key, Column(Order = 0)]
        public int outletID { get; set; }
        [Key, Column(Order = 1)]
        public string barcode { get; set; }
        public decimal sellingPrice { get; set; }
        public int currentStock { get; set; }
        public int minimumStock { get; set; }
        public int discountPercentage { get; set; }
        public int temporaryStock { get; set; }
        public int afterUpdateStock { get; set; }
       
    }
}
