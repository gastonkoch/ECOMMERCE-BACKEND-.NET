using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ProductCreateRequest
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(400)]
        public string Description { get; set; }

        [Required]
        public int Stock { get; set; }

        public static Product ToEntity(ProductCreateRequest dto)
        {
            Product product = new Product();
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Stock = dto.Stock;
            return product;
        }
    }
}
