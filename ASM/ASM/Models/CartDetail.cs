using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ASM.Models
{
    public partial class CartDetail
    {
        public int Stt { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantily { get; set; }
        public decimal? Total { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
