using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HQServer.WebUI.Models
{
    public class OutletsListViewModel
    {
        public IEnumerable<Outlet> Outlets { get; set; }
        public DotNet.Highcharts.Highcharts chart { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}