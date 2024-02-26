using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject
{
    public partial class Service
    {
        public Service()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public Guid? HostId { get; set; }
        public int? DeleteFlag { get; set; }

        public virtual Account Host { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
