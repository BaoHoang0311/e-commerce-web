using e_commerce_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class ProfileVM
    {
        public ProfileVM()
        {
            OrderList = new();
        }
        public Customer customer { get; set; }
        public ChangePasswordVM changepass { get; set; }
        public List<Order> OrderList { get; set; }
    }
}
