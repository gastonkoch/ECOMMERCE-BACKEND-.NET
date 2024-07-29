using Application.Models;
using Application.Models.Requests;
using Application.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        OrderWhitPriceResponse GetOrderById(int id);
        OrderDto CreateOrder(OrderCreateRequest order);
        List<OrderWhitPriceResponse> GetAllOrders();
        Task UpdateOrderProduct(int orderId, ChangeOrderRequest changeOrder);
        ICollection<OrderDto> GetOrdersBySeller(int sellerId);
        ICollection<OrderDto> GetOrdersByCustomer(int customerId);
        Task<string> DeleteOrder(int id);
        void ChangeOrderStatus(int orderId, ChangeStatusOrderRequest changeOrderStatus);
        string CheckStockProduct(int productId);
    }
}
