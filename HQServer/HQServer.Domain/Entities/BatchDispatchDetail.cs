using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class BatchDispatchDetail
    {
        [Key, Column(Order = 0)]
        public int batchDispatchID { get; set; }
        [Key, Column(Order = 1)]
        public int barcode { get; set; }
        public int quantity { get; set; }
    }
}
