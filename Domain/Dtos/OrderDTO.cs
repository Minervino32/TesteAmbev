using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class OrderDTO
    {
        public required string Id { get; set; }

        [NotMapped]
        public required List<Product> Products { get; set; } = new List<Product>();
    }
}
