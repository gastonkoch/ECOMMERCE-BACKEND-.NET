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
    public class OrderProductRepository : EfRepository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<List<OrderProduct>> GetListOrdersWithProduct(int ProductId)
        {
            return await _applicationDbContext.OrdersProducts.Where(p => p.Product.Id == ProductId).ToListAsync();
        }
        public virtual async Task<List<OrderProduct>> GetListOrdersByOrderId(int OrderId)
        {
            return await _applicationDbContext.OrdersProducts.Where(p => p.Order.Id == OrderId).ToListAsync();
        }
    }
}
