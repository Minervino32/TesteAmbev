using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class OrderProcessedViewModel
    {
        public string Id { get; set; }
        public string Requestor { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusDescription { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
