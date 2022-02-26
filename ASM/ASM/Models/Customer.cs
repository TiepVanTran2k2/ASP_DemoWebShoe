using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ASM.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Cart = new HashSet<Cart>();
        }

        public int CustomerId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PassWord { get; set; }
        public bool Status { get; set; }
        public int Role { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
    }
}
