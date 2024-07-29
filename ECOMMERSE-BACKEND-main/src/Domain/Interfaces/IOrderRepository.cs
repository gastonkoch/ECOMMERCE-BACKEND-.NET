using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<Order?> ChangeStatusOrder(int id, StatusOrder newStatusOrder, CancellationToken cancellationToken = default);
        Task<List<Order>> OrdersBySellers(int sellerId, CancellationToken cancellationToken = default);
        Task<List<Order>> OrdersByCustomer(int customerId, CancellationToken cancellationToken = default);
    }
}
