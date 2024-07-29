using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly IEmailService _emailService;

        public SendEmailService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void SendEmail(string to, string subject, string body)
        {
            _emailService.SendEmail(to, subject, body);
        }
    }
}
