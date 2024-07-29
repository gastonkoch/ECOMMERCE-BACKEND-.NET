using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record ProductsInOrderDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductsInOrderDto(int id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}
