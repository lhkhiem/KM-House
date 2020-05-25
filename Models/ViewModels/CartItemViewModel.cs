using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{[Serializable]
    public class CartItemViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
