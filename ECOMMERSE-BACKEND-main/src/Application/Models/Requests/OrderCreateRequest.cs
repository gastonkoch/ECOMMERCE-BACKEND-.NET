using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class OrderCreateRequest
    {
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]

        public int CustomerId { get; set; }

        [Required]

        public int SellerId { get; set; }

        [Required]
        public IEnumerable<ProductsInOrderDto> ProductsInOrder { get; set; } // Utilizar dto Id + amount
    }
}
