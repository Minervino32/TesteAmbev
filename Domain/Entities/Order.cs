using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateCreated { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Imported;

        [NotMapped]
        public List<Product>? Products { get; set; }
    }
}
