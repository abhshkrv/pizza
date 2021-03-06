﻿using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HQServer.WebUI.Models
{
    public class OutletInventoryViewModel
    {
        public IEnumerable<OutletInventory> Inventory { get; set; }
        public DotNet.Highcharts.Highcharts chart { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int currentOutletID { get; set; }
    }
}