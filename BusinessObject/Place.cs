using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Place
    {
        public Place()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int? Price { get; set; }
        public Guid? HostId { get; set; }
        public int? DeleteFlag { get; set; }

        public virtual Account Host { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
