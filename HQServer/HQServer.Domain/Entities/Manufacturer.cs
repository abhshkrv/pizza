using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class Manufacturer
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int manufacturerID { get; set; }
        public string manufacturerName { get; set; }
    }
}
