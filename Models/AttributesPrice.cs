using System;
using System.Collections.Generic;

#nullable disable

namespace e_commerce_web.Models
{
    public partial class AttributesPrice
    {
        public int AttributesPriceId { get; set; }
        public int? AttributeId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price { get; set; }
        public bool Active { get; set; }

        public virtual Attribute Attribute { get; set; }
        public virtual Product Product { get; set; }
    }
}
