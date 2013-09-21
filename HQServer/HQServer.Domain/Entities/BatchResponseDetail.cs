using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class BatchResponseDetail
    {
        public int requestID { get; set; }
        public int barcode { get; set; }
        public int quantity { get; set; }
    }
}
