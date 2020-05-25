using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public long ProductID { get; set; }
        public long OrderID { get; set; }
        [StringLength(250)]
        public string ProductName { get; set; }
        [StringLength(250)]
        public string ProductImage { get; set; }

        public int Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
