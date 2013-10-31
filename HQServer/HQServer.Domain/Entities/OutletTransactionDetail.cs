using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class OutletTransactionDetail
    {
        [Key, Column(Order = 0)]
        public int transactionSummaryID { get; set; }
        [Key, Column(Order = 1)]
        public int outletID { get; set; }
        [Key, Column(Order = 2)]
        public int barcode { get; set; }
        public int unitSold { get; set; }
        public float cost { get; set; }
    }
}
