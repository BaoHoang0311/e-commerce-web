﻿using System;
using System.Collections.Generic;

#nullable disable

namespace e_commerce_web.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderNumber { get; set; }
        public int? Amount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalMoney { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
