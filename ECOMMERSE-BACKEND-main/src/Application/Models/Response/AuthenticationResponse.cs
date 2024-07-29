using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class AuthenticationResponse(string token)
    {
        public string Token { get; set; } = token;
    }
}
