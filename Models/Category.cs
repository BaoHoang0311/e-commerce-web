using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace e_commerce_web.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CatId { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập")]
        public string CatName { get; set; }
        public bool Published { get; set; }
        public string Thumb { get; set; }
        public string Alias { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập")]
        public string Cover { get; set; }
        public string SchemaMarkup { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
