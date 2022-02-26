using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Models
{
    public class ProductViewModel : EditImageViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Introduce { get; set; }
        public int SupplierId { get; set; }
        public bool? Status { get; set; }
        //public IFormFile Images { get; set; }
        public int Quantity { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
