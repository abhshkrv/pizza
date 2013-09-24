using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class Outlet
    {
        public int outletID { get; set; }
        public string owner { get; set; }
        public string address { get; set; }

    }
}
