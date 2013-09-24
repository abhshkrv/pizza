using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HQServer.Domain.Entities
{
    public class Member
    {
        public int membershipID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public int hpNumber { get; set; }
        public string password { get; set; }
        public int points { get; set; }
    }
}
