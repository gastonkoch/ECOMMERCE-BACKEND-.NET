using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async override Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var existingProduct = await _applicationDbContext.Products.FindAsync(new object[] { product.Id }, cancellationToken);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"El producto {product.Id} no se encontro.");
            }

            _applicationDbContext.Entry(existingProduct).CurrentValues.SetValues(product);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<List<Product>> GetListAvaible()
        {
            return await _applicationDbContext.Products.Where(p => p.ProductAvaible == true).ToListAsync();
        }

        public virtual async Task<List<Product>> GetByName(string name)
        {
           string nameLower = name.ToLower();
            return await _applicationDbContext.Products.Where(p => p.Name.ToLower().Contains(nameLower)).ToListAsync();
        }
    }
}
