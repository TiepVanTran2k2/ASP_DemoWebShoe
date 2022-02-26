using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ASM.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Product = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
