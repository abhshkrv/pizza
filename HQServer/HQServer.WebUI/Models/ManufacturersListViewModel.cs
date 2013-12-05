using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HQServer.WebUI.Models
{
    public class ManufacturersListViewModel
    {
        public IEnumerable<Manufacturer> Manufacturers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}