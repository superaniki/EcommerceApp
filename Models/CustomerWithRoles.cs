using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApp.Models
{
    public class CustomerWithRoles
    {
        public Customer Customer { get; set; }
        public IList<string> Roles { get; set; }
    }
}