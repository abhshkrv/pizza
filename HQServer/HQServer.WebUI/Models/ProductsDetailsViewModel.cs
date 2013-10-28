using HQServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HQServer.WebUI.Models
{
    public class ProductsDetailsViewModel
    {
        public Product product { get; set; }
        public Manufacturer manufacturer { get; set; }
        public Category category { get; set; }

    }
}