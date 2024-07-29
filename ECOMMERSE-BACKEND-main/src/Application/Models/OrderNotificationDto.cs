using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class OrderNotificationDto
    {
        [Required]
        public string Message { get; set; }

        public static OrderNotificationDto todto(OrderNotificationDto ordernotification)
        {
            OrderNotificationDto dto = new OrderNotificationDto();
            {
                dto.Message = ordernotification.Message;
                return dto;
            }
        }
    }
}
