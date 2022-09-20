using e_commerce_web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class ListCategoryVM
    {
        public ListCategoryVM()
        {
            ListCate = new();
        }
        public List<Category_check> ListCate { get; set; }
    }
    public class Category_check
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}
