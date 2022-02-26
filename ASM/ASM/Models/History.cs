using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Models
{
    public class History
    {
       public CustomerDetail CustomerDetail { get; set; }
       public Product product { get; set; }
       public Cart Cart { get; set; }
    }
}
