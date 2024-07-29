using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderProductRepository : IRepositoryBase<OrderProduct>
    {
        Task<List<OrderProduct>> GetListOrdersWithProduct(int ProductId);
        Task<List<OrderProduct>> GetListOrdersByOrderId(int OrderId);
    }
}
