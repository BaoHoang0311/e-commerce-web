using e_commerce_web.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class ListCartItemVM
    {
        public ListCartItemVM()
        {
            ListCart = new();
        }
        public List<CartItemVM> ListCart { get; set; }
        public decimal TongTien { get; set; }
    }
}
