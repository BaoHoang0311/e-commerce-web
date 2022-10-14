using System;
using System.Collections.Generic;

#nullable disable

namespace e_commerce_web.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
            Customers = new HashSet<Customer>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
