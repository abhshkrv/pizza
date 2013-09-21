﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class BatchResponse
    {
        public int requestID { get; set; }
        public int outletID { get; set; }
        public DateTime timestamp { get; set; }
        public string comments { get; set; }
        public Status status { get; set; }
    }
    public enum Status
    {
        NOT_RESPONDED,
        RESPONDED,
        ACKNOWLEDGED
    }
}