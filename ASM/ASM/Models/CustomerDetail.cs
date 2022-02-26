using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ASM.Models
{
    public partial class CustomerDetail
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string DeliveryAddress { get; set; }
        public string Phone { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
