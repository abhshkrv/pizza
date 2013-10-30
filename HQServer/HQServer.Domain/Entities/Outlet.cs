using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace HQServer.Domain.Entities
{
    public class Outlet
    {
        [HiddenInput(DisplayValue = false)]
        public int outletID { get; set; }
        public string owner { get; set; }
        public string address { get; set; }

    }
}
