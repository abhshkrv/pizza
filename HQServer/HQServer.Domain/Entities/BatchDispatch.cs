using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class BatchDispatch
    {
        [Key]
        public int batchDispatchID { get; set; }
        public int outletID { get; set; }
        public DateTime timestamp { get; set; }
        public string comments { get; set; }
        public DispatchStatus status { get; set; }
    }
    public enum DispatchStatus
    {
        NOT_RESPONDED,
        RESPONDED,
        ACKNOWLEDGED
    }
}
