using e_commerce_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class ThanhToanVM
    {
        public ThanhToanVM()
        {
            ListCartItem = new();
            cus = new();
        }
        public Customer cus { get; set; }
        public ListCartItemVM ListCartItem { get; set; }
    }
}
