using e_commerce_web.Data.Services;
using e_commerce_web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class CartItemVM
    {
        public CartItemVM()
        {
            sanpham = new Product();
        }
        public Product sanpham { get; set; }
        public int amount { get; set; }
        public decimal Tien { get; set; }
        public decimal Total()
        {
            var tien = amount * sanpham.Price;
            Tien = tien.Value;
            return Tien;
        }
    }
}
