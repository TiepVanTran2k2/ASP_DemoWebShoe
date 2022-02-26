using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ASM.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? Status { get; set; }
        public decimal? SumPrice { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string NameRecipient { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
