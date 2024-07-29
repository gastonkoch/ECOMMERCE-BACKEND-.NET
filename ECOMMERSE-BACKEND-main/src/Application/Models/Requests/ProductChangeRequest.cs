using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ProductChangeRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public static Product ToEntity(ProductChangeRequest dto)
        {
            Product product = new Product();
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            return product;
        }
    }
}
