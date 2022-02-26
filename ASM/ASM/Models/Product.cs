using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ASM.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Introduce { get; set; }
        public int SupplierId { get; set; }
        public bool? Status { get; set; }
        public string Images { get; set; }
        public int Quantity { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
