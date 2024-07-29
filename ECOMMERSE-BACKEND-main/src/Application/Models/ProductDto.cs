using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public bool ProductAvaible { get; set; } = true;

        public static ProductDto ToDto(Product product)
        {
            ProductDto productDto = new ProductDto();
            productDto.Id = product.Id;
            productDto.Name = product.Name;
            productDto.Price = product.Price;
            productDto.Description = product.Description;
            productDto.Stock = product.Stock;
            productDto.ProductAvaible = product.ProductAvaible;
            return productDto;
        }

        public static Product ToEntity(ProductDto dto)
        {
            Product product = new Product();
            product.Id = dto.Id;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Stock = dto.Stock;
            product.ProductAvaible = dto.ProductAvaible;
            return product;
        }

        public static List<ProductDto> ToList(IEnumerable<Product> products)
        {
            List<ProductDto> listProductDto = new List<ProductDto>();

            foreach (var product in products)
            {
                listProductDto.Add(ProductDto.ToDto(product));
            }
            return listProductDto;
        }
    }
}
