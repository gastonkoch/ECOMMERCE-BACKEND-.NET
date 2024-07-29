using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISendEmailService
    {
        public void SendEmail(string to, string subject, string body);
    }
}
