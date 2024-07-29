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
    public class OrderNotificationRepository : EfRepository<OrderNotification>, IOrderNotificationRepository
    {
        public OrderNotificationRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<List<OrderNotification>> GetListNotificationsByOrderId(int OrderId)
        {
            return await _applicationDbContext.OrderNotifications.Where(p => p.Order.Id == OrderId).ToListAsync();
        }
    }
}
