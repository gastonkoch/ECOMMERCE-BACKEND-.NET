using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        public AuthenticationResponse Autenticate(AuthenticationRequest authenticationRequest);
    }
}
