using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class SearchVM
    {
        public string Cat { get; set; }
        public string keySearch { get; set; }
        public int? A { get; set; }
        public int? B {get;set;}
        public string sortOrder { get; set; }
    }
}
