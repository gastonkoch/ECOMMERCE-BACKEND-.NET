using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ChangeOrderRequest
    {
        // Order Header 
        public int PaymentMethod { get; set; }
        public int StatusOrder { get; set; }
        // Order Product
        public List<ProductsInOrderDto> ProductsInOrder { get; set; }
    }

}
