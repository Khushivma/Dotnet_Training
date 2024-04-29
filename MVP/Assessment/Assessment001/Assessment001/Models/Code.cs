using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment001.Models
{
    public class Code
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}