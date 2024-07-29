using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderNotificationService
    {
        OrderNotificationDto GetOrderNotificationById(int id);
        OrderNotificationDto CreateOrderNotification(OrderNotificationCreateRequest dto);
    }
}
