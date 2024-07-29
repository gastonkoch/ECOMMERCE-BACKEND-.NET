using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class OrderProductDto
    {
        public int OrderProductId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }

        public static OrderProductDto ToDto(OrderProduct orderProduct)
        {
            OrderProductDto dto = new OrderProductDto();
            dto.OrderProductId = orderProduct.OrderProductId;
            dto.ProductId = orderProduct.Product.Id;
            dto.Name = orderProduct.Product.Name;
            dto.Price = orderProduct.Product.Price;
            dto.Description = orderProduct.Product.Description;
            dto.Stock = orderProduct.Product.Stock;
            dto.Quantity = orderProduct.Quantity;
            return dto;
        }

        public static OrderProduct ToEntity(OrderProductDto dto)
        {
            OrderProduct orderProduct = new OrderProduct();
            orderProduct.OrderProductId = dto.OrderProductId;
            orderProduct.Product.Id = dto.ProductId;
            orderProduct.Product.Name = dto.Name;
            orderProduct.Product.Price = dto.Price;
            orderProduct.Product.Description = dto.Description;
            orderProduct.Product.Stock = dto.Stock;
            orderProduct.Quantity = dto.Quantity;
            return orderProduct;
        }

        public static ICollection<OrderProductDto> ToList(ICollection<OrderProduct> orderProducts)
        {
            List<OrderProductDto> listOrderProductDto = new List<OrderProductDto>();

            foreach (var product in orderProducts)
            {
                listOrderProductDto.Add(OrderProductDto.ToDto(product));
            }
            return listOrderProductDto;
        }



    }
}
