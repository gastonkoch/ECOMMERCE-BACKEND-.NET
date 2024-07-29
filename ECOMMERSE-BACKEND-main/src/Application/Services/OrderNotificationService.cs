using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderNotificationService
    {
        private readonly IOrderNotificationRepository _orderNotificationService;

        public OrderNotificationService(IOrderNotificationRepository orderNotificationService)
        {
            _orderNotificationService = orderNotificationService;
        }
    }

}
