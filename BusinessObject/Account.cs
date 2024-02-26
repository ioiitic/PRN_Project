using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
            Places = new HashSet<Place>();
            Services = new HashSet<Service>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int? Role { get; set; }
        public int? DeleteFlag { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Place> Places { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
