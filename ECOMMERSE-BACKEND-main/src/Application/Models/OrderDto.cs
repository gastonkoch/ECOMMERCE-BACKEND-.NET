using Application.Models.Requests;
using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public UserDto Customer { get; set; }
        public UserDto Seller { get; set; }
        public ICollection<OrderProductDto>? ProductsInOrder { get; set; }

        public static OrderDto ToDto(Order order)
        {
            OrderDto dto = new OrderDto();
            dto.Id = order.Id;
            dto.PaymentMethod = order.PaymentMethod;
            dto.StatusOrder = StatusOrder.InProgress;
            dto.Customer = UserDto.ToDto(order.Customer);
            dto.Seller = UserDto.ToDto(order.Seller);
            dto.ProductsInOrder = OrderProductDto.ToList(order.ProductsInOrder);
            return dto;
        }

        public static Order ToEntity(OrderDto dto)
        {
            Order order = new Order();
            order.Id = dto.Id;
            order.PaymentMethod = dto.PaymentMethod;
            order.StatusOrder = dto.StatusOrder;
            return order;
        }

        public static List<OrderDto> ToList(IEnumerable<Order> orders)
        {
            List<OrderDto> listOrderDto = new List<OrderDto>();

            foreach (var order in orders)
            {
                listOrderDto.Add(OrderDto.ToDto(order));
            }
            return listOrderDto;
        }
    }
}
