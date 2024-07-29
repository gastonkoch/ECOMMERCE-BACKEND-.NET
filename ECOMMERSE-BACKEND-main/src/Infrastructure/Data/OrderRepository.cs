using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Orders
                 .Include(o => o.Customer)
                 .Include(o => o.Seller)
                 .Include(o => o.ProductsInOrder)
                     .ThenInclude(op => op.Product)
                 .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Orders
                 .Include(o => o.Customer)
                 .Include(o => o.Seller)
                 .Include(o => o.ProductsInOrder)
                     .ThenInclude(op => op.Product)
                 .ToListAsync(cancellationToken);
        }

        public async Task<Order?> ChangeStatusOrder(int id, StatusOrder newStatusOrder , CancellationToken cancellationToken = default)
        {
            var order = await _applicationDbContext.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            order.StatusOrder = newStatusOrder;

            _applicationDbContext.Orders.Update(order);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return order;
        }

        public virtual async Task<List<Order>> OrdersByCustomer(int customerId, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Orders
                 .Include(o => o.Customer)
                 .Include(o => o.Seller)
                 .Include(o => o.ProductsInOrder)
                     .ThenInclude(op => op.Product)
                     .Where(o => o.Customer.Id == customerId)
                 .ToListAsync(cancellationToken);
        } 

        public virtual async Task<List<Order>> OrdersBySellers(int sellerId, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Orders
                 .Include(o => o.Customer)
                 .Include(o => o.Seller)
                 .Include(o => o.ProductsInOrder)
                     .ThenInclude(op => op.Product)
                     .Where(o => o.Seller.Id == sellerId)
                 .ToListAsync(cancellationToken);
        }
    }
}
