using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class OutletTransactionDetail
    {
        public int transactionID { get; set; }
        public int outletID { get; set; }
        public int barcode { get; set; }
        public int unitSold { get; set; }
        public float cost { get; set; }
    }
}
