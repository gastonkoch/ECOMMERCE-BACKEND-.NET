using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class OrderWhitPriceResponse
    {
        public int Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public UserDto Customer { get; set; }
        public UserDto Seller { get; set; }
        public decimal TotalOrder { get; set; } = 0;
        public ICollection<OrderProductWhitPriceResponse>? ProductsInOrder { get; set; } = new List<OrderProductWhitPriceResponse>();
    }
}
