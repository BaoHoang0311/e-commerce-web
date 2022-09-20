using e_commerce_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class HomePageVM
    {
        public HomePageVM()
        {
            All = new();
            TraiCayNhapKhau = new();
            RauCu = new();
        }
        public List<Product> All { get; set; }
        public List<Product> TraiCayNhapKhau { get; set; }
        public List<Product> RauCu { get; set; }

    }
}
