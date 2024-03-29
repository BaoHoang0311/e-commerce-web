﻿using System;
using System.Collections.Generic;

#nullable disable

namespace e_commerce_web.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? LocationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Active { get; set; }
        public int? RoleId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
