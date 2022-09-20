using e_commerce_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class NewProductVM
    {
        public NewProductVM()
        {
            LsTopProduct = new();
        }
        public List<Product> LsTopProduct { get; set; }
    }
}
