using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class OnlineTransactionDetail
    {
        [Key, Column(Order = 0)]
        public int transactionID { get; set; }
        [Key, Column(Order = 2)]
        public string barcode { get; set; }
        public int shopID { get; set; }
        public int unitSold { get; set; }
        public decimal totalCost { get; set; }
    }
}