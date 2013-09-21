using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class OutletTransaction
    {
        public string transactionID { get; set; }
        public DateTime date { get; set; }
        public int outletID { get; set; }
    }
}
